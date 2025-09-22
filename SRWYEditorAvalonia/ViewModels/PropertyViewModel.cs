using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Ursa.Controls;

namespace SRWYEditorAvalonia.ViewModels
{
    public abstract partial class NodeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string displayName;
        [ObservableProperty]
        private bool isNative = true;

        protected NodeViewModel? container;
        public NodeViewModel? Container => container;

        public ObservableCollection<NodeViewModel> Children { get; } = new();

        private bool isChildrenLoaded = false;
        public bool IsChildrenLoaded => isChildrenLoaded;

        [ObservableProperty]
        private bool isExpanded;
        public bool IsListElement => container != null && container is CollectionNodeViewModel;
        public bool Removeable => IsListElement && IsNative == false;

        partial void OnIsExpandedChanged(bool value)
        {
            if (value && !isChildrenLoaded)
            {
                isChildrenLoaded = true;
                LoadChildren();
            }
        }
        public void ForceLoadChildren()
        {
            if (!isChildrenLoaded)
            {
                isChildrenLoaded = true;
                LoadChildren();
            }
        }

        /// <summary>
        /// 子类重写此方法以实现懒加载
        /// </summary>
        protected virtual void LoadChildren() { }
        public virtual void RemoveChild(NodeViewModel child) { }


    }

    /// <summary>
    /// 用于在懒加载中充当占位符的虚拟节点
    /// </summary>
    public class DummyNodeViewModel : NodeViewModel
    {
        public DummyNodeViewModel()
        {
            DisplayName = "Loading...";
        }
    }

    // --- 具体节点 ViewModel ---

    /// <summary>
    /// 代表一个可编辑的、简单的属性（叶子节点）
    /// </summary>
    public partial class PropertyNodeViewModel : NodeViewModel
    {
        [ObservableProperty]
        private object value;

        private readonly PropertyInfo propertyInfo;
        private readonly object owner;

        public PropertyNodeViewModel(object owner, PropertyInfo propertyInfo, NodeViewModel? container = null)
        {
            this.owner = owner;
            this.propertyInfo = propertyInfo;
            DisplayName = propertyInfo.Name;
            this.value = propertyInfo.GetValue(owner);
            this.container = container;
        }

        partial void OnValueChanged(object value)
        {
            if (propertyInfo.CanWrite)
            {
                try
                {
                    // 处理可空类型和常规类型转换
                    var targetType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    var convertedValue = value == null ? null : Convert.ChangeType(value, targetType);
                    propertyInfo.SetValue(owner, convertedValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to set property {propertyInfo.Name}: {ex.Message}");
                }
            }
        }
        
    }
    /// <summary>
    /// 专门用于表示枚举类型属性的节点
    /// </summary>
    public class EnumPropertyNodeViewModel : PropertyNodeViewModel
    {
        public IEnumerable EnumValues { get; }

        public EnumPropertyNodeViewModel(object owner, PropertyInfo propertyInfo, NodeViewModel? container = null)
            : base(owner, propertyInfo)
        {
            EnumValues = Enum.GetValues(propertyInfo.PropertyType);
            this.container = container;
        }
    }

    /// <summary>
    /// 代表一个复杂的对象（可展开的父节点）
    /// </summary>
    public partial class ObjectNodeViewModel : NodeViewModel
    {
        private readonly object sourceInstance;
        public object SourceInstance => sourceInstance;

        public ObjectNodeViewModel(string name, object obj, NodeViewModel? container = null)
        {
            DisplayName = name;
            sourceInstance = obj;
            this.container = container;

            if (sourceInstance != null && sourceInstance.GetType() != typeof(string))
            {
                Children.Add(new DummyNodeViewModel());
            }
        }

        protected override void LoadChildren()
        {
            if (sourceInstance != null && sourceInstance.GetType() != typeof(string))
            {
                Children.Clear();
                var childNodes = ObjectEditorViewModelFactory.BuildViewModelNodes(sourceInstance, this);
                foreach (var childNode in childNodes)
                {

                    Children.Add(childNode);
                }
            }
                
        }
        [RelayCommand]
        public async Task Remove()
        {
            var result = await MessageBox.ShowAsync("删除后无法撤销，确认删除该元素？", "警告", MessageBoxIcon.Warning, MessageBoxButton.OKCancel);
            if (result.HasFlag(MessageBoxResult.OK))
            {
                container?.RemoveChild(this);
            }
        }
    }

    /// <summary>
    /// 代表一个集合或数组（可展开的父节点）
    /// </summary>
    public partial class CollectionNodeViewModel : NodeViewModel
    {
        private readonly IList collection;

        public int Count => collection.Count;

        public bool Resizeable => !collection.IsFixedSize && !collection.IsReadOnly;

        public CollectionNodeViewModel(string name, IList collection, NodeViewModel? container = null)
        {
            DisplayName = name;
            this.collection = collection;
            this.container = container;
            Children.Add(new DummyNodeViewModel());
        }

        [RelayCommand(CanExecute = nameof(Resizeable))]
        private void AddChild()
        {
            Type? t = GetElementType();
            if (t is not null)
            {
                var item = ObjectFactory.CreateInstanceAndPopulateFields(t);
                
                int index = collection.Count;
                string name = GetItemName(item, index);
                var itemNode = ObjectEditorViewModelFactory.CreateNode(name, item, container: this);
                itemNode.IsNative = false;
                collection.Add(item);
                Children.Add(itemNode);
                OnPropertyChanged(nameof(Count));
            }
        }
        private Type? GetElementType()
        {
            if (collection.GetType().IsGenericType)
            {
                return collection.GetType().GenericTypeArguments[0];
            }
            return null;
        }
        private string GetItemName(object item, int index)
        {
            string name = $"[{index}]";
            var prop = item.GetType().GetProperty("identificationName");
            if (prop is not null)
            {
                var idName = prop.GetValue(item) as string;
                if (!string.IsNullOrEmpty(idName))
                {
                    name = $"[{index}] {idName}";
                }
            }
            else
            {
                prop = item.GetType().GetProperty("nameJP");
                if (prop is not null)
                {
                    var idName = prop.GetValue(item) as string;
                    if (!string.IsNullOrEmpty(idName))
                    {
                        name = $"[{index}] {idName}";
                    }
                }
                else
                {
                    prop = item.GetType().GetProperty("IdentificationName");
                    if (prop is not null)
                    {
                        var idName = prop.GetValue(item) as string;
                        if (!string.IsNullOrEmpty(idName))
                        {
                            name = $"[{index}] {idName}";
                        }
                    }
                }
            }
            return name;
        }

        protected override void LoadChildren()
        {
            Children.Clear();
            int index = 0;
            foreach (var item in collection)
            {
                string name = GetItemName(item, index);
                var itemNode = ObjectEditorViewModelFactory.CreateNode(name, item, container:this);
                Children.Add(itemNode);
                index++;
            }
        }

        public override void RemoveChild(NodeViewModel child)
        {
            if (child is ObjectNodeViewModel objNode && collection.Contains(objNode.SourceInstance))
            {
                var index = collection.IndexOf(objNode.SourceInstance);
                collection.Remove(objNode.SourceInstance);
                Children.Remove(objNode);
                for (int i = index; i < Children.Count; i++)
                {
                    var item = Children[i];
                    item.DisplayName = GetItemName(((ObjectNodeViewModel)item).SourceInstance, i);
                }
                OnPropertyChanged(nameof(Count));
            }
        }
    }

    // --- 工厂类 ---

    /// <summary>
    /// 使用反射来构建 ViewModel 树的静态工厂
    /// </summary>
    public static class ObjectEditorViewModelFactory
    {
        public static ObservableCollection<NodeViewModel> BuildViewModelNodes(object instance, NodeViewModel? container = null)
        {
            var nodes = new ObservableCollection<NodeViewModel>();
            if (instance == null) return nodes;

            var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (!prop.CanRead) continue;

                var value = prop.GetValue(instance);
                nodes.Add(CreateNode(prop.Name, value, instance, prop, container));
            }
            return nodes;
        }

        public static NodeViewModel CreateNode(string name, object value, object? owner = null, PropertyInfo? propInfo = null, NodeViewModel? container = null)
        {
            if (value == null)
            {
                return owner != null && propInfo != null
                    ? new PropertyNodeViewModel(owner, propInfo, container)
                    : new ObjectNodeViewModel(name, null, container) { DisplayName = $"{name}: null" };
            }

            var type = value.GetType();

            if (typeof(IList).IsAssignableFrom(type) && type != typeof(string))
            {
                
                return new CollectionNodeViewModel(name, (IList)value, container);
            }

            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type.IsEnum)
            {
                if (owner != null && propInfo != null)
                {
                    return type.IsEnum
                        ? new EnumPropertyNodeViewModel(owner, propInfo, container)
                        : new PropertyNodeViewModel(owner, propInfo, container);
                }
                
                return new ObjectNodeViewModel($"{name}: {value}", value, container);
            }

            return new ObjectNodeViewModel(name, value, container);
        }
    }

    public static class ObjectFactory
    {
        /// <summary>
        /// 创建指定类型的实例，并自动初始化其所有引用类型的字段。
        /// </summary>
        /// <typeparam name="T">要创建的类型</typeparam>
        /// <returns>一个已初始化部分字段的 T 类型实例</returns>
        public static T CreateInstanceAndPopulateFields<T>() where T : class
        {
            return (T)CreateInstanceAndPopulateFields(typeof(T));
        }

        /// <summary>
        /// 创建指定类型的实例，并自动初始化其所有引用类型的字段。
        /// </summary>
        /// <param name="type">要创建的类型</param>
        /// <returns>一个已初始化部分字段的对象</returns>
        public static object CreateInstanceAndPopulateFields(Type type)
        {
            // 1. 创建主对象实例
            // 要求该类型必须有无参数的构造函数
            object instance = Activator.CreateInstance(type);

            // 2. 获取所有实例字段（包括 public 和 non-public）
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // 3. 遍历所有字段
            foreach (var field in fields)
            {
                // 如果字段的值已经是被设置过的（例如在构造函数中），则跳过
                if (field.GetValue(instance) != null && !field.FieldType.IsValueType)
                {
                    continue;
                }

                Type fieldType = field.FieldType;
                object fieldValue = null;

                // 4. 判断字段类型并创建实例
                if (fieldType == typeof(string))
                {
                    // 为 string 类型赋一个空字符串
                    fieldValue = string.Empty;
                }
                // 判断是否是泛型List<>
                else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // 直接创建这个List<T>类型的实例
                    fieldValue = Activator.CreateInstance(fieldType);
                }
                // 判断是否是其他类 (并且不是 string)，且有无参构造函数
                else if (fieldType.IsClass && fieldType.GetConstructor(Type.EmptyTypes) != null)
                {
                    // 递归调用，以便也能初始化这个嵌套对象的字段
                    fieldValue = CreateInstanceAndPopulateFields(fieldType);
                }

                // 值类型(struct, int, bool...) 和没有无参构造函数的类将保持其默认值 (0, false, null)

                if (fieldValue != null)
                {
                    // 5. 将创建的实例赋值给主对象的字段
                    field.SetValue(instance, fieldValue);
                }
            }

            return instance;
        }
    }
}
