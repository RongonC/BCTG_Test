
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class CommonMethods
    {
        #region Methods

        //#region Create Case For Form
        //public static void CreateCaseForForm(int itemInstanceTranId, string currentUser)
        //{
        //    try
        //    {
        //        Boolean blnCMEFlag = true;
        //        Boolean blnSendEmail = false;
        //        string strSupervisorEmail = string.Empty;
        //        string strCreateCaseWhen = string.Empty;
        //        string strMessage = string.Empty;
        //        string strIsProcessCMECase = string.Empty;
        //        List<ItemInstanceTranData> lsItemInstanceTranData = new List<ItemInstanceTranData>();
        //        lsItemInstanceTranData.AddRange(DataAccessQuest.GetItemInstanceTranData(itemInstanceTranId)); // get item instance tran data
        //        if (lsItemInstanceTranData.Count > 0)
        //        {
        //            strSupervisorEmail = lsItemInstanceTranData[0].strSupervisorEmail.ToString();
        //        }

        //        List<ItemInfoField> lstItemInfoField = new List<ItemInfoField>();
        //        lstItemInfoField = DataAccessQuest.GetItemInfoFieldsAssocSecByItemID(lsItemInstanceTranData[0].intItemID, "VIEW", currentUser);

        //        foreach (var item in lstItemInfoField)
        //        {
        //            blnSendEmail = item.blnSuppressAlert.GetValueOrDefault();
        //            strIsProcessCMECase = item.strIsProcessCMECase;
        //        }
        //        ///////////// start Process CME Cases
        //        if (strIsProcessCMECase.ToUpper() == "Y") // 
        //        {
        //            if (blnCMEFlag == true)
        //            {// add CME cases for Prop QA in case of form is locked/finalized. and add CME cases for other items for form is lock or unlock. 
        //                String strQueueName_ProcessCMECases = System.Configuration.ConfigurationManager.AppSettings["ProcessCMECases_QueuePath"].ToString();
        //                string strMSMQMesg_ProcessCMECases = string.Empty;
        //                strMSMQMesg_ProcessCMECases = itemInstanceTranId + "|INSERT|" + strSupervisorEmail;
        //                LogWriter.LogWrite("CreateCaseForForm ProcessCMECases_QueuePath : " + strMSMQMesg_ProcessCMECases + strQueueName_ProcessCMECases + itemInstanceTranId);
        //                AddMsgToQueue(strMSMQMesg_ProcessCMECases, strQueueName_ProcessCMECases, itemInstanceTranId.ToString());
        //            }
        //        }
        //        ///////////// End Process CME Cases
        //        // send alert
        //        if (blnSendEmail == true && string.IsNullOrEmpty(strSupervisorEmail) == false /*&& intItemId != intTenantSurveyItemID*/)
        //        {
        //            String strQueueName_SendEmailAlert = System.Configuration.ConfigurationManager.AppSettings["SendEmailAlert_QueuePath"].ToString();
        //            string strMSMQMesg_SendEmailAlert = string.Empty;
        //            strMSMQMesg_SendEmailAlert = itemInstanceTranId + "|INSERT|" + strSupervisorEmail;
        //            LogWriter.LogWrite("CreateCaseForForm SendEmailAlert_QueuePath : " + strMSMQMesg_SendEmailAlert + strQueueName_SendEmailAlert + itemInstanceTranId);
        //            AddMsgToQueue(strMSMQMesg_SendEmailAlert, strQueueName_SendEmailAlert, itemInstanceTranId.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region Create Case For Update Form
        //public static void CreateCaseForUpdateForm(int itemInstanceTranId, string currentUser)
        //{
        //    try
        //    {
        //        Boolean blnCMEFlag = true;
        //        Boolean blnSendEmail = false;
        //        string strSupervisorEmail = string.Empty;
        //        string strCreateCaseWhen = string.Empty;
        //        string strMessage = string.Empty;
        //        string strIsProcessCMECase = string.Empty;
        //        List<ItemInstanceTranData> lsItemInstanceTranData = new List<ItemInstanceTranData>();
        //        lsItemInstanceTranData.AddRange(DataAccessQuest.GetItemInstanceTranData(itemInstanceTranId)); // get item instance tran data
        //        if (lsItemInstanceTranData.Count > 0)
        //        {
        //            strSupervisorEmail = lsItemInstanceTranData[0].strSupervisorEmail.ToString();
        //        }

        //        List<ItemInfoField> lstItemInfoField = new List<ItemInfoField>();
        //        lstItemInfoField = DataAccessQuest.GetItemInfoFieldsAssocSecByItemID(lsItemInstanceTranData[0].intItemID, "VIEW", currentUser);

        //        foreach (var item in lstItemInfoField)
        //        {
        //            blnSendEmail = item.blnSuppressAlert.GetValueOrDefault();
        //            strIsProcessCMECase = item.strIsProcessCMECase;
        //        }
        //        ///////////// start Process CME Cases
        //        if (strIsProcessCMECase.ToUpper() == "Y") // 
        //        {
        //            if (blnCMEFlag == true)
        //            {// add CME cases for Prop QA in case of form is locked/finalized. and add CME cases for other items for form is lock or unlock. 
        //                String strQueueName_ProcessCMECases = System.Configuration.ConfigurationManager.AppSettings["ProcessCMECases_QueuePath"].ToString();
        //                string strMSMQMesg_ProcessCMECases = string.Empty;
        //                strMSMQMesg_ProcessCMECases = itemInstanceTranId + "|UPDATE|" + strSupervisorEmail;
        //                LogWriter.LogWrite("CreateCaseForForm ProcessCMECases_QueuePath : " + strMSMQMesg_ProcessCMECases + strQueueName_ProcessCMECases + itemInstanceTranId);
        //                AddMsgToQueue(strMSMQMesg_ProcessCMECases, strQueueName_ProcessCMECases, itemInstanceTranId.ToString());
        //            }
        //        }
        //        ///////////// End Process CME Cases
        //        // send alert
        //        if (blnSendEmail == true && string.IsNullOrEmpty(strSupervisorEmail) == false /*&& intItemId != intTenantSurveyItemID*/)
        //        {
        //            String strQueueName_SendEmailAlert = System.Configuration.ConfigurationManager.AppSettings["SendEmailAlert_QueuePath"].ToString();
        //            string strMSMQMesg_SendEmailAlert = string.Empty;
        //            strMSMQMesg_SendEmailAlert = itemInstanceTranId + "|UPDATE|" + strSupervisorEmail;
        //            LogWriter.LogWrite("CreateCaseForForm SendEmailAlert_QueuePath : " + strMSMQMesg_SendEmailAlert + strQueueName_SendEmailAlert + itemInstanceTranId);
        //            AddMsgToQueue(strMSMQMesg_SendEmailAlert, strQueueName_SendEmailAlert, itemInstanceTranId.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region Add Msg To Queue
        //public static void AddMsgToQueue(string strMessage, string strQueueName, string strMsgLabel)
        //{
        //    try
        //    {
        //        LogWriter.LogWrite("AddMsgToQueue : " + strMessage + strQueueName + strMsgLabel);
        //        LogWriter.LogWrite("Message Queue exist : " + IsQueueAvailable(strQueueName));
        //        System.Messaging.MessageQueue msqQ = new System.Messaging.MessageQueue(strQueueName);
        //        msqQ.Send(strMessage, strMsgLabel);
        //        LogWriter.LogWrite("AddMsgToQueue Completed");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogWriter.LogWrite("AddMsgToQueue Error : " + ex.GetBaseException().InnerException);
        //        LogWriter.LogWrite("AddMsgToQueue Error : " + ex.GetBaseException().StackTrace);
        //        LogWriter.LogWrite("AddMsgToQueue Error : " + ex.GetBaseException().Message);
        //    }
        //}
        //#endregion

        //#region Queue Available or not
        //public static bool IsQueueAvailable(string queueName)
        //{
        //    var queue = new System.Messaging.MessageQueue(queueName);
        //    try
        //    {
        //        queue.Peek(new TimeSpan(0, 0, 5));
        //        return true;
        //    }
        //    catch (System.Messaging.MessageQueueException ex)
        //    {
        //        return ex.Message.StartsWith("Timeout");
        //    }
        //}
        //#endregion

        #region GenerateStreamFromString
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;

        }
        #endregion

        #endregion
    }
}