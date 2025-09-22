using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SRWYEditor.Models
{
    [System.AttributeUsage(AttributeTargets.Property)]
    public class UABEAPropertyTypeNameAttribute : Attribute
    {
        public string Name { get;}

        public UABEAPropertyTypeNameAttribute(string name)
        {
            Name = name;
        }
    }
    public class PilotBasicDatas
    {
        public PPtr<GameObject> m_GameObject { get; set; }
        public byte m_Enabled { get; set; }
        public PPtr<MonoScript> m_Script { get; set; }
        public string m_Name { get; set; }
        public List<PilotBaseData> listPilotBaseData { get; set; }
        public List<HaveSpiritCommandData> listHaveSpiritCommandData { get; set; }
        public List<BirthdaySettingData> listBirthdaySettingData { get; set; }
        public List<ZodiacSignData> listZodiacSignData { get; set; }
        public List<FacialSettingData> listFacialSettingData { get; set; }
        public List<FacialChangeSettingData> listFacialChangeSettingData { get; set; }
        public List<AceBonusData> listAceBonusData { get; set; }
        public List<MoraleAddedValueData> listMoraleAddedValueData { get; set; }
        public List<SpiritCommandData> listSpiritCommandData { get; set; }
        public List<ShotdownMessageData> listShotdownMessageData { get; set; }
        public List<AceTalkSettingData> listAceTalkSettingData { get; set; }
        public List<HavePilotSkillData> listHavePilotSkillData { get; set; }
        public List<PilotSkillCategoryData> listPilotSkillCategoryData { get; set; }
        public List<SkillProgramData> listSkillProgramData { get; set; }
        public List<KakeaiData> listKakeaiData { get; set; }
        public List<PilotSkillData> listPilotSkillData { get; set; }
        public List<AddedValueForEachLevelData> listAddedValueForEachLevelData { get; set; }
    }
    public class PilotBaseData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string facialId { get; set; }
        public string defaultReferenceId { get; set; }
        public int sortNumber { get; set; }
        public PilotValueSet parameters { get; set; }
        public Gender genderType { get; set; }
        public Species speciesType { get; set; }
        public string moraleType { get; set; }
        public string growType { get; set; }
        public int rewardExp { get; set; }
        public byte onHeart { get; set; }
        public OtherElementForPilot[] otherElements { get; set; }
        public TransferType[] transferTypes { get; set; }
        public string aceBonusId { get; set; }
        public string MAPWLines { get; set; }
        public string bvcFile { get; set; }
        public byte bvcVersion { get; set; }
        public byte introSkip { get; set; }
        public string voiceContainer { get; set; }
        public SAInterface aceBonus { get; set; }
        public List<GrowPilotSkill> growSkills { get; set; }
        public List<GrowSpiritCommand> growSpirits { get; set; }
        public FacialSettingData characterData { get; set; }
        public byte isCommonSoldier { get; set; }
        public int shotdownMessageNo { get; set; }
        public string voiceActor { get; set; }
    }
    public class PilotValueSet
    {
        public int melee { get; set; }
        public int range { get; set; }
        public int defend { get; set; }
        public int skill { get; set; }
        public int evade { get; set; }
        public int hit { get; set; }
        public int sp { get; set; }
        public enum Type
        {
            Melee,
            Ranged,
            Defend,
            Hit,
            Evade,
            Skill,
            SP
        }
    }

    public class SAInterface
    {
        public string IdentificationName { get; set; }
        public string AbilityId { get; set; }
        public string CategoryId { get; set; }
        [UABEAPropertyTypeName("Keys")]
        public SAInterfaceKeys Key { get; set; }
        [UABEAPropertyTypeName("Options")]
        public SAInterfaceOptions Option { get; set; }
        [UABEAPropertyTypeName("Texts")]
        public SAInterfaceTexts Text { get; set; }
        public List<SASet> Sets { get; set; }
    }
    public class SAInterfaceTexts
    {
        public string NameJP { get; set; }
        public string DescriptionJP { get; set; }
        public string DescriptionShortJP { get; set; }
    }
    public class SAInterfaceKeys
    {
        public string NameKey { get; set; }
        public string DescriptionKey { get; set; }
        public string DescriptionShortKey { get; set; }
        public string TargetNameKey { get; set; }
        public string TerminationTimingNameKey { get; set; }
    }
    public class SAInterfaceOptions
    {
        public int Level { get; set; }
        public int Version { get; set; }
        public int UsableCount { get; set; }
        public byte IsDummy { get; set; }
        public SAGroup Group { get; set; }
        public SAFilter Filter { get; set; }
        public RobotSkillIcon RobotSkillIcon { get; set; }
    }
    public class SASet
    {
        public string AnyMemo { get; set; }
        public SATiming ConditionTiming { get; set; }
        public SARelationship ConditionRelationship { get; set; }
        public byte OnDrawActivated { get; set; }
        public List<SAJudge> Judges { get; set; }
        public List<SAEffect> Effects { get; set; }
    }
    public class SAJudge
    {
        public string StringForList { get; set; }
        public SAJudgeMethods.Key MethodKey { get; set; }
        public int Value { get; set; }
        [UABEAPropertyTypeName("OtherSettings")]
        public SAJudgeOtherSettings OtherSetting { get; set; }

        
    }
    public class SAJudgeOtherSettings
    {
        public SpiritFlag SpiritCommandFlag { get; set; }
        public byte OrJudge { get; set; }
        public string TargetId { get; set; }
        public OtherElementForPilot PilotElement { get; set; }
        public ButtonHandler.ButtonType UnitCommandType { get; set; }
        public ButtonHandler.SpecialType UnitCommandSpecialType { get; set; }
        public byte WithSlotIndex { get; set; }
        public byte CheckDefaultId { get; set; }
        public byte CheckMounted { get; set; }
        public byte isPicMAPW { get; set; }
        public byte isMathEqual { get; set; }
        public byte isIncludingMyself { get; set; }
        public byte isGetterMenber { get; set; }
    }
    public class SAEffect
    {
        public string StringForList { get; set; }
        public SAEffectMethodsKey MethodKey { get; set; }
        public int Value { get; set; }
        public WeaponDebuffSetting WeaponDebuffs { get; set; }
        [UABEAPropertyTypeName("OtherSettings")]
        public SAEffectOtherSettings OtherSetting { get; set; }

        public enum WeaponSelectType
        {
            All,
            Psycommu,
            Aura,
            ShootingType,
            MeleeType,
            SlotNumber,
            MAPW,
            NotMAPW,
            NotMAPWAndRangeOtherThanOne,
            NotMAPWShootingType,
            NotMAPWFightType,
            NotUnitedWeaponType
        }

        
    }
    public class SAEffectOtherSettings
    {
        public SpiritFlag SpiritCommandFlag { get; set; }
        public SAGroup AbilityGroup { get; set; }
        public string TargetId { get; set; }
        public string ExchangeId { get; set; }
        public SAEffect.WeaponSelectType WeaponSelect { get; set; }
        public int SelectNumber { get; set; }
        public LandAdaptation.Type LandAdaptationSelect { get; set; }
        public LandAdaptationRank LandAdaptationSetRank { get; set; }
        public int BaseParamChange { get; set; }
        public int Range { get; set; }
        public byte OnUseCorrectListPawnCount { get; set; }
        public byte OnUseCorrectListPilotCount { get; set; }
        public byte OnUseCorrectListEnemyShotDown { get; set; }
        public byte WithSlotIndex { get; set; }
        public byte OnLastCritical { get; set; }
        public byte OnSubPilotEffect { get; set; }
        public byte OnHitMorals { get; set; }
    }
    public class WeaponDebuffSetting
    {
        public byte EN { get; set; }
        public byte Armor { get; set; }
        public byte Mobility { get; set; }
        public byte Sight { get; set; }
        public byte Morale { get; set; }
        public byte SP { get; set; }
        public byte CutPilotParameterInHalf { get; set; }
        public byte ShutDown { get; set; }
    }
    public class SAFilter
    {
        public byte Robot { get; set; }
        public byte Weapon { get; set; }
        public byte Move { get; set; }
        public byte Spirit { get; set; }
        public byte Consume { get; set; }
        public byte Get { get; set; }
        public byte Special { get; set; }
    }
    public class GrowPilotSkill
    {
        public string skillId { get; set; }
        public int[] mastaryLevels { get; set; }
    }
    public class GrowSpiritCommand
    {
        public string id { get; set; }
        public int mastaryLevel { get; set; }
        public int cost { get; set; }
        public int befotcost { get; set; }
    }
    public class FacialSettingData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string defaultReferenceId { get; set; }
        public string nameKey { get; set; }
        public string fullNameKey { get; set; }
        public string nameJP { get; set; }
        public string fullNameJP { get; set; }
        public byte onProtagonist { get; set; }
        public byte onRegistDictionary { get; set; }
        public string ipnameId { get; set; }
        public byte onReverse { get; set; }
        public List<FacialSetting> faces { get; set; }
        public List<FacialSetting> bustups { get; set; }
    }
    public class FacialSetting
    {
        public int number { get; set; }
        public AssetReference spriteReference { get; set; }
    }
    public class HaveSpiritCommandData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public List<GrowSpiritCommand> settings { get; set; }
    }
    public class BirthdaySettingData
    {
        public string id { get; set; }
        public List<GrowSpiritCommand> settings { get; set; }
    }
    public class ZodiacSignData
    {
        public string id { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }
    public class FacialChangeSettingData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string targetFacialId { get; set; }
        public string changeFicialId { get; set; }
        public string conditionFlag { get; set; }
        public byte onDictionary { get; set; }
        public byte onScenario { get; set; }
        public int changeFacialNumber { get; set; }
    }
    public class AceBonusData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string descriptionJP { get; set; }
    }
    public class MoraleAddedValueData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public int shotDown { get; set; }
        public int allyShotDownAnEnemy { get; set; }
        public int allysShotDown { get; set; }
        public int avoid { get; set; }
        public int hit { get; set; }
        public int attacked { get; set; }
        public int miss { get; set; }
    }
    public class SpiritCommandData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string iconKey { get; set; }
        public string descriptionJP { get; set; }
        public string descriptionShortJP { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
        public string descriptionShortKey { get; set; }
        public string exitTypeKey { get; set; }
        public string targetKey { get; set; }
        public byte onFlag { get; set; }
        public byte onSelf { get; set; }
        public byte onBulk { get; set; }
        public byte onOther { get; set; }
        public byte onBattleNoMove { get; set; }
        public byte onBattleOnMove { get; set; }
        public byte onBattleReserve { get; set; }
        public byte onBattleCounter { get; set; }
        public byte onActionEnd { get; set; }
        public SpiritCommandUnusableStateData UnusableStateData { get; set; }
    }
    public class SpiritCommandUnusableStateData
    {
        public byte MoraleMax { get; set; }
        public byte MoraleMin { get; set; }
        public byte HPMax { get; set; }
        public byte SPMax { get; set; }
        public byte ENMaxAndBulletMax { get; set; }
        public byte AssistGaugeMax { get; set; }
        public byte MountFlag { get; set; }
        public byte DebuffFlag { get; set; }
        public enum Type
        {
            MoraleMax,
            MoraleMin,
            HPMax,
            SPMax,
            ENMaxAndBulletMax,
            AssistGaugeMax,
            MountFlag,
            DebuffFlag
        }
    }
    public class ShotdownMessageData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string textKey { get; set; }
        public int serifuNo { get; set; }
    }
    public class AceTalkSettingData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string[] targetBasisIds { get; set; }
        public string addedSkillProgram { get; set; }
    }
    public class HavePilotSkillData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public List<GrowPilotSkill> settings { get; set; }
    }
    public class PilotSkillCategoryData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string descriptionJP { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
    }
    public class SkillProgramData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string descriptionJP { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
        public int cost { get; set; }
        public SkillProgramRarity rarity { get; set; }
        public string targetCategory { get; set; }
        public string type { get; set; }
    }
    public class KakeaiData
    {
        public string id { get; set; }
        public string voiceGroup { get; set; }
        public string[] pilotBasisIds { get; set; }
    }
    public class PilotSkillData
    {
        public string identificationName { get; set; }
        public string id { get; set; }
        public string categoryId { get; set; }
        public int level { get; set; }
        public int updateCost { get; set; }
        public int version { get; set; }
        public string descriptionJP { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
        public int rarity { get; set; }
        public byte DummyFlag { get; set; }
    }
    public class AddedValueForEachLevelData
    {
        public string id { get; set; }
        public List<PilotValueSet> dataForEachLevels { get; set; }
    }
}