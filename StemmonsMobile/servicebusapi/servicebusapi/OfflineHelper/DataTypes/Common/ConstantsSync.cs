using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Common
{
    public class ConstantsSync
    {
        // public static string Baseurl = "";
        public static int INSTANCE_USER_ASSOC_ID = 1;

        public static string CasesInstance = "CASES";
        public static string UserProfileInstance = "USERPROFILE";
        public static string UserDetailsInstance = "USERDETAILS";
        public static string EntityInstance = "ENTITY";
        public static string QuestInstance = "QUEST";
        public static string StandardInstance = "STANDARD";
        public static string Home = "HOME";

        //DO NOT BRAEK SEQUENCE.. ADD NEW ITEM AT LAST INDEX ALWAYS IT WILL CAUSE YOUR APP MAY NOT WORK PROPERLY
        public enum ActionTypes
        {
            /*0*/
            CREATECASE,
            /*1*/
            ADDNOTES,
            /*2*/
            FORWARDCASE,
            /*3*/
            ASSIGNCASE,
            /*4*/
            CREATE_ENITY,
            /*5*/
            SAVE_ENTITY,
            /*6*/
            CREATEFORM,
            /*7*/
            UPDATEFORM,
            /*8*/
            UPDATECASE,
            /*9*/
            CREATEFAVORITE,
            /*10*/
            ENTITYTAKEOWNERSHIP,
            /*11*/
            CASESTAKEOWNERSHIP,
            /*12*/
            CASESAPPROVEANDRETURN,
            /*13*/
            CASESRETURNTOLASTASSIGNEE,
            /*14*/
            CASESRETURNTOLASTASSIGNER,
            /*15*/
            CASESAPPROVEANDASSIGN,
            /*16*/
            CASESDECLINEANDRETURN,
            /*17*/
            CASEDECLINEANDASSIGN,
            /*18*/
            CASESCLOSE,
            /*19*/
            CASESACTIVITYLOG,
            /*20*/
            ASSIGNENTITY,
            /*21*/
            FORWARDENTITY,
            /*22*/
            DELETEENTITY,
            /*23*/
            ADDCASETOQUESTION

        }

        public enum Applications
        {
            Quest = 1, Cases = 2, Entities = 3, Departments = 1002, Standards = 2002
        }

        #region Cases Offline API Lists
        public static string GetAllCaseTypeWithID = "/api/v1/Synchronize/GetAllCaseTypeWithID";
        #endregion

        #region Entity Offline API Lists
        public static string GetEntityTypeList = "/api/v1/Synchronize/FillEntityOfflineData";
        #endregion

        #region Standards Offline API Lists

        public static string GetAllStandards = "/api/v1/Synchronize/GetAllStandards";
        #endregion

        #region Quest Offline API Lists
        public static string GetAllQuestTypeWithID = "/api/v1/Synchronize/GetAllQuest";
        #endregion

        #region ScreenConstants

        public static string HomeScreenCount = "B1_HomeScreenCount";
        public static string Entity_Category_TypeDetails = "G1_G2_Entity_Cate_TypeDetails";
        public static string EntityType_CountDetails = "G3_EntityType_CountDetails";
        public static string EntityItemList_Lazyload = "G4_EntityitemList_Lazyload";
        public static string EntityItemView = "G8_EntityitemView";
        public static string EntityRelatedApplications = "G8_EntityRelatedApplications";
        public static string EntityRelatedTypesList = "G10_GetEntityRelatedTypes";
        public static string EntityRelatedData = "G10_GetEntitiesRelationData";
        public static string CasesRelatedData = "G10_GetCasesRelationData";
        public static string QuestRelatedData = "G10_GetQuestRelationData";
        public static string EntityItemNotes = "G8_EntityItemNotes";
        public static string TakeOwnership_G11 = "G11_EntityTakeownerShip";
        public static string MyEntityAssociation = "MyEntityAssociationList";
        public static string EntityItemAssign_G11 = "G11_EntityAssign";
        public static string EntityItemForward_G11 = "G11_EntityForward";

        public static string GetUserInfo = "Home_Screen_Department_GetUserInfo";
        public static string GetQuestFormsForUser = "Home_Screen_Quest_GetQuestFormsForUser";
        public static string GetCasesListUser = "Home_Screen_Cases_CasesHomeGetCaseListUser";
        public static string GetAllAppForUser = "Home_Screen_Cases_GetAllAppForUser";
        public static string GetCasesEntityList = "Home_Screen_Cases_GetEntityList";
        public static string GetTeamMembers = "Home_Screen_Department_GetTeamMembers";
        public static string security_SearchMetadata = "security_SearchMetadata";

        public static string GetHopperWithOwnerByUsername = "GetHopperWithOwnerByUsername";

        public static string GetCaseNoteTypes = "K3_GetCaseNoteTypes";
        public static string GetAllEntityRoleRelationshipByEmp = "K3_GetAllEntityRoleRelationshipByEmp";

        //Standard
        public static string GetBookDetailToUser = "I1_GetBookDetailToUsern";
        public static string BookView = "I2_BookView";

        public static string C8_GetTypesByCaseTypeIDRaw = "C8_GetTypesByCaseTypeIDRaw";

        public static string B1_GetBooklistPublishedByUserName = "B1_GetPublishedAppByUserBasedOnSAM";

        public static string B1_GetBooklistCreatedByUserName = "B1_GetAppCreatedByUserBasedOnSAM";

        #endregion

    }
}
