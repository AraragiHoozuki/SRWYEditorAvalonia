using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SRWYEditor.Models
{
    public class GameObject;
    public class MonoScript;
    public class PPtr<T>
    {
        public int m_FileID { get; set; }
        public Int64 m_PathID { get; set; }
    }
    public class RobotBasicDatas
    {
        public PPtr<GameObject> m_GameObject { get; set; }
        public byte m_Enabled { get; set; }
        public PPtr<MonoScript> m_Script { get; set; }
        public string m_Name { get; set; }
        public List<RobotBaseData> listRobotBaseData { get; set; }
        public List<OnlyPieceData> listOnlyPieceData { get; set; }
        public List<SpecialMoveData> listSpecialMoveData { get; set; }
        public List<RetrofitData> listRetrofitData { get; set; }
        public List<AwakeData> listAwakeData { get; set; }
        public List<TransformationData> listTransformationData { get; set; }
        public List<MapTransformationData> listMapTransformationData { get; set; }
        public List<ArmorPurgeData> listArmorPurgeData { get; set; }
        public List<UnitedData> listUnitedData { get; set; }
        public List<SeparatedData> listSeparatedData { get; set; }
        public List<FullCustomBonusData> listFullCustomBonusData { get; set; }
        public List<CustomBonusData> listCustomBonusData { get; set; }
        public List<SpecialDefenseData> listSpecialDefenseData { get; set; }
        public List<RobotSkillData> listRobotSkillData { get; set; }
        public List<RobotSkillCategoryData> listRobotSkillCategoryData { get; set; }
        public List<RobotUpgradeCostData> listRobotUpgradeCostData { get; set; }
        public List<HaveRobotSkillData> listHaveRobotSkillData { get; set; }
        public List<WeaponUpgradeCostData> listWeaponUpgradeCostData { get; set; }
        public List<PowerPartsData> listPowerPartsData { get; set; }
    }
    public class RobotBaseData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string defaultReferenceId { get; set; }
        public string robotimageID { get; set; }
        public string displayNameKey { get; set; }
        public string formalNameKey { get; set; }
        public string displayNameJP { get; set; }
        public string formalNameJP { get; set; }
        public string ipnameId { get; set; }
        public int sortNumber { get; set; }
        public RobotValueSet parameters { get; set; }
        public int movePower { get; set; }
        public RobotSize size { get; set; }
        public int powerPartsSlotNumber { get; set; }
        public LandAdaptation landAdaptation { get; set; }
        public RobotValueSet upgradeParameters { get; set; }
        public string weaponUpgradeCostType { get; set; }
        public int rewardCapital { get; set; }
        public int rewardExp { get; set; }
        public byte onProtagonist { get; set; }
        public byte onShip { get; set; }
        public byte onIgnoreLandAdaptationUpgrade { get; set; }
        public byte onRegistDictionary { get; set; }
        public TransferType transferType { get; set; }
        public string customBonusId { get; set; }
        public string defaultBGM { get; set; }
        public string editBGMLink { get; set; }
        public string mapPiece { get; set; }
        public byte besideDouble { get; set; }
        public string localizeKey { get; set; }
        public string animeId { get; set; }
        public string battleSE { get; set; }
        public string voiceGroup { get; set; }
        public string voiceContainer { get; set; }
        public List<WeaponBaseData> weapons { get; set; }
        public List<RobotSkillData> skills { get; set; }
        public List<SpecialMoveData> specialMoves { get; set; }
        public List<RetrofitData> retrofits { get; set; }
        public List<AwakeData> awakes { get; set; }
        public List<TransformationData> transformations { get; set; }
        public MapTransformationData mapTransformation { get; set; }
        public List<ArmorPurgeData> armorPurges { get; set; }
        public List<UnitedData> uniteds { get; set; }
        public List<SeparatedData> separateds { get; set; }
        public RobotImageSet images { get; set; }
    }

    public class RobotValueSet
    {
        public int hp { get; set; }
        public int en { get; set; }
        public int armor { get; set; }
        public int mobility { get; set; }
        public int sight { get; set; }
        public int weapon { get; set; }
    }

    public class LandAdaptation
    {
        public LandAdaptationRank sky { get; set; }
        public LandAdaptationRank ground { get; set; }
        public LandAdaptationRank water { get; set; }
        public LandAdaptationRank space { get; set; }

        public enum Type
        {
            Sky,
            Ground,
            Water,
            Space
        }
    }

    public class WeaponBaseData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string belongRobotBasisId { get; set; }
        public string displayNameJP { get; set; }
        public string formalNameJP { get; set; }
        public string displayNameKey { get; set; }
        public string formalNameKey { get; set; }
        public int slotNumber { get; set; }
        public string unitedKey { get; set; }
        public string conditionDisplayFlag { get; set; }
        public string conditionPilot { get; set; }
        public string conditionBasisPilot { get; set; }
        public byte onHidden { get; set; }
        public byte onCustomBonus { get; set; }
        public byte onUpgradeBonus { get; set; }
        public WeaponHandlingType handlingType { get; set; }
        public string upgradeType { get; set; }
        public int power { get; set; }
        public int rangeMin { get; set; }
        public int rangeMax { get; set; }
        public int sight { get; set; }
        public int critical { get; set; }
        public int bullet { get; set; }
        public int costEn { get; set; }
        public int conditionMorale { get; set; }
        public LandAdaptation landAdaptation { get; set; }
        public byte onMapwFriendlyFire { get; set; }
        public MAPWFiringType mapwFiringType { get; set; }
        public string mapwMatrix { get; set; }
        public string mapwAnime { get; set; }
        public int mapwRange { get; set; }
        public WeaponPowerType powerType { get; set; }
        public WeaponDebuffType debuffType { get; set; }
        public byte onCounter { get; set; }
        public byte onUseMoveAfter { get; set; }
        public byte onBarrierPenetration { get; set; }
        public byte onIgnoreSize { get; set; }
        public string conditionSkillKey { get; set; }
        public string bgmKey { get; set; }
        public string bgmLinkKey { get; set; }
        public string battleSE { get; set; }
        public string fullAnm { get; set; }
        public int fullVoiceNumber { get; set; }
        public int2_storage[] mapwMatrixValues { get; set; }
        public UnitedWeaponData unitedWeaponData { get; set; }
    }

    public struct int2_storage
    {
        public int x { get; set; }
        public int y { get; set; }
    }
    public class UnitedWeaponData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public int additionalPower { get; set; }
        public List<UnitedWeaponSetting> settings { get; set; }
    }
    public class UnitedWeaponSetting
    {
        public int weaponSlot { get; set; }
        public string robotBasisId { get; set; }
        public string conditionPilotBasisId { get; set; }
    }

    public class RobotSkillData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string categoryId { get; set; }
        public string nameJP { get; set; }
        public string descriptionJP { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
    }
    public class SpecialMoveData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string pieceName { get; set; }
        public MovingType movingType { get; set; }
        public string effectName { get; set; }
    }
    public class RetrofitData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string selectNameKey { get; set; }
        public string[] basisIds { get; set; }
    }
    public class AwakeData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public AwakeType awakeType { get; set; }
        public string commandNameKey { get; set; }
        public int conditionMorale { get; set; }
        public string conditionPSkillKey { get; set; }
        public string conditionPilotBasisId { get; set; }
        public string useFlag { get; set; }
        public string afterRobotBasisId { get; set; }
        public string afterPilotBasisId { get; set; }
        public byte onRecoverAwake { get; set; }
        public byte onRecoverCommand { get; set; }
        public string animetionKey { get; set; }
    }
    public class TransformationData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string commandNameKey { get; set; }
        public List<TransformationMemberData> members { get; set; }
    }
    public class TransformationMemberData
    {

        public string robotBasisId { get; set; }
        public string pilotReferenceId { get; set; }
    }
    public class MapTransformationData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string landRobotBasisId { get; set; }
        public string spaceRobotBasisId { get; set; }
    }
    public class ArmorPurgeData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string commandNameKey { get; set; }
        public string basisId { get; set; }
    }
    public class UnitedData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string commandNameKey { get; set; }
        public string unitedBasisId { get; set; }
        public string[] memberBasisIds { get; set; }
        public string afterPilotBasisId { get; set; }
    }
    public class SeparatedData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string commandNameKey { get; set; }
        public string[] basisIds { get; set; }
    }
    public class RobotImageSet
    {
        public AssetReference FaceImage { get; set; }
        public AssetReference FullImage { get; set; }
        public AssetReference ListImage { get; set; }
    }

    public class AssetReference
    {
        public string m_AssetGUID { get; set; }
        public string m_SubObjectName { get; set; }
        public string m_SubObjectType { get; set; }
    }
    public class OnlyPieceData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string pieceName { get; set; }
    }
    public class FullCustomBonusData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
        public string nameJP { get; set; }
        public string descriptionJP { get; set; }
    }
    public class CustomBonusData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string descriptionJP { get; set; }
    }
    public class SpecialDefenseData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string Anime { get; set; }
        public string battleSE { get; set; }
        public string voiceContainer { get; set; }
        public int ENCost { get; set; }
        public int ActivationProbability { get; set; }
        public int ReduceDamage { get; set; }
        public int PreventDamage { get; set; }
        public SpecialDefenseType FunctionType { get; set; }
        public WeaponHandlingType ConditionHandlingType { get; set; }
        public WeaponPowerType ConditionPowerType { get; set; }
    }
    public class RobotSkillCategoryData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string descriptionJP { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
    }
    public class RobotUpgradeCostData
    {
        public string id { get; set; }
        public int[] costs { get; set; }
    }
    public class HaveRobotSkillData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string[] rskillIds { get; set; }
    }
    public class WeaponUpgradeCostData
    {

        public string id { get; set; }
        public int[] costs { get; set; }
    }
    public class PowerPartsData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string descriptionJP { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
        public int sellingPrice { get; set; }
        public PowerPartsRarity rarity { get; set; }
        public int usableCount { get; set; }
        public string targetKey { get; set; }
        public byte useableWithSupplyParts { get; set; }
        public PowerPartsFilter filter { get; set; }
        public PowerPartsCommandUnusableStateData UnusableStateData { get; set; }
    }
    public class PowerPartsFilter
    {
        public byte Robot { get; set; }
        public byte Weapon { get; set; }
        public byte Move { get; set; }
        public byte Spirit { get; set; }
        public byte Consume { get; set; }
        public byte Get { get; set; }
        public byte Special { get; set; }
    }
    public class PowerPartsCommandUnusableStateData
    {
        public byte MoraleMax { get; set; }
        public byte MoraleMin { get; set; }
        public byte HPMax { get; set; }
        public byte SPMax { get; set; }
        public byte MainSPMax { get; set; }
        public byte ENMax { get; set; }
        public byte BulletMax { get; set; }
        public byte AssistGaugeMax { get; set; }
        public byte SpiritCommandFlag { get; set; }
        public byte DebuffFlag { get; set; }
        public enum Type
        {
            MoraleMax,
            MoraleMin,
            HPMax,
            SPMax,
            MainSPMax,
            ENMax,
            BulletMax,
            AssistGaugeMax,
            SpiritCommandFlag,
            DebuffFlag
        }
    }
}