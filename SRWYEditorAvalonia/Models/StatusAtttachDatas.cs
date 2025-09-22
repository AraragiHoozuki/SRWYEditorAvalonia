using SRWYEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.Models
{
    public class StatusAttachDatas
    {
        public PPtr<GameObject> m_GameObject { get; set; }
        public byte m_Enabled { get; set; }
        public PPtr<MonoScript> m_Script { get; set; }
        public string m_Name { get; set; }
        public List<SACategoricalInformation> listPilotSkillCategoricalInformation { get; set; }
        public List<SAInterface> listRobotSkill { get; set; }
        public List<SAInterface> listPowerParts { get; set; }
        public List<SAInterface> listAceBonus { get; set; }
        public List<SAInterface> listCustomBonus { get; set; }
        public List<SAInterface> listFullCustomBonus { get; set; }
        public List<SAInterface> listAllyEffect { get; set; }
        public List<SACategoricalInformation> listAssistPassiveCategoricalInformation { get; set; }
        public List<SACategoricalInformation> listAssistActiveCategoricalInformation { get; set; }
        public List<SACategoricalInformation> listSpiritCommandCategoricalInformation { get; set; }
    }

    public class SACategoricalInformation
    {
        public string nameJP { get; set; }
        public string categoryId { get; set; }
        public string nameKey { get; set; }
        public string descriptionKey { get; set; }
        public string descriptionJP { get; set; }
        public int maxLevel { get; set; }
        public List<SAInterface> list { get; set; }
    }
}