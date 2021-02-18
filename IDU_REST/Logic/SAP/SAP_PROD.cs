using IDU_REST.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using IDU_REST.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IDU_REST.Logic.SAP
{
    public class SAP_PROD
    {
        public List<PROD> GetDataList(SAPbobsCOM.Company oCompany, DateTime FromDate, DateTime ToDate)
        {
            Utils control = new Utils();
            List<PROD> details = new List<PROD>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "select T0.DocEntry,T0.DocNum as 'FormNumber',T0.Status,T0.PostDate as 'OrderDate',T0.DueDate as 'DueDate',T0.Warehouse, " +
                         " T0.ItemCode as 'ProductNo',T0.ProdName as 'ProductDescription',T0.PlannedQty as 'PlanedQty',T0.Uom, " +
                         " T0.OcrCode as 'ProjectCode',T1.OcrName as 'ProjectName',T0.Comments as 'Remarks',T0.CardCode as 'VendorCode',T2.CardName as 'VendorName'" +
                         " from OWOR T0 inner join OOCR T1 on T0.OcrCode = T1.OcrCode INNER JOIN OCRD T2 on T2.CardCode = T0.CardCode" +
                         " where T0.StartDate between '" + FromDate + "' and '" + ToDate + "'";
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            List<PROD> model = (from data in xDoc.Element("BOM").Element("BO").Element("OWOR").Elements("row")
                                select new PROD
                                {

                                    DocEntry = Convert.ToInt32(data.Element("DocEntry").Value),
                                    FormNumber = data.Element("FormNumber").Value,
                                    VendorCode = data.Element("VendorCode").Value,
                                    VendorName = data.Element("VendorName").Value,
                                    Status = data.Element("Status").Value,
                                    warehouse = data.Element("Warehouse").Value,
                                    ProductNo = data.Element("ProductNo").Value,
                                    ProductDescription = data.Element("ProductDescription").Value,
                                    plannedQty = Convert.ToDecimal(data.Element("PlanedQty").Value),
                                    UOM = data.Element("Uom").Value,
                                    ProjectCode = data.Element("ProjectCode").Value,
                                    ProjectName = data.Element("ProjectName").Value,
                                    Remarks = data.Element("Remarks").Value,
                                    OrderDate = DateTime.ParseExact(data.Element("OrderDate").Value, "yyyyMMdd",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.None),
                                    DueDate = DateTime.ParseExact(data.Element("DueDate").Value, "yyyyMMdd",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.None),


                                }).ToList();

            return model;
        }

        public static PROD GetData(SAPbobsCOM.Company oCompany, int id)
        {
            Utils control = new Utils();
            List<PROD_DETIL> details = new List<PROD_DETIL>();
            XDocument xDoc = new XDocument();
            XDocument xDocDetail = new XDocument();

            string sql = "select T0.DocEntry,T0.DocNum as 'FormNumber',T0.Status,T0.PostDate as 'OrderDate',T0.DueDate as 'DueDate',T0.Warehouse, " +
                         " T0.ItemCode as 'ProductNo',T0.ProdName as 'ProductDescription',T0.PlannedQty as 'PlanedQty',T0.Uom, " +
                         " T0.OcrCode as 'ProjectCode',T1.OcrName as 'ProjectName',T0.Comments as 'Remarks'" +
                         " from OWOR T0 inner join OOCR T1 on T0.OcrCode = T1.OcrCode " +
                         " where T0.DocEntry =  " + id;
            Recordset rs = control._IDU_Recordset(oCompany, sql);
            xDoc = XDocument.Parse(rs.GetAsXML());

            sql = "select  T1.DocEntry, T1.LineNum,T1.ItemCode,T2.ItemName,T1.BaseQty,T1.PlannedQty,T1.IssuedQty,T1.IssueType,T1.wareHouse,T1.Project " +
                " from WOR1 T1 inner join OITM T2 on T2.ItemCode = T1.ItemCode  where T1.DocEntry = " + id;
            rs = control._IDU_Recordset(oCompany, sql);
            xDocDetail = XDocument.Parse(rs.GetAsXML());

            XElement xEle = xDoc.Element("BOM").Element("BO").Element("OWOR").Element("row");
            XElement xEleDetail = xDocDetail.Element("BOM").Element("BO").Element("WOR1");

            foreach (var item in xEleDetail.Elements("row"))
            {
                PROD_DETIL model_detail = new PROD_DETIL()
                {
                    ItemCode = item.Element("ItemNo").Value,
                    ItemName = item.Element("ItemName").Value,
                    BaseQty = Convert.ToDecimal(item.Element("BaseQty").Value),
                    PlannedQty = Convert.ToDecimal(item.Element("PlannedQty").Value),
                    IssueQty = Convert.ToDecimal(item.Element("Price").Value),
                    warehouse = item.Element("WhsCode").Value,
                    project = item.Element("GLAccount").Value,


                };

                details.Add(model_detail);
            }

            PROD model = new PROD()
            {

                DocEntry = Convert.ToInt32(xEle.Element("DocEntry").Value),
                FormNumber = xEle.Element("FormNumber").Value,
                Status = xEle.Element("Status").Value,
                warehouse = xEle.Element("OrderDate").Value,
                ProjectCode = xEle.Element("ProjectCode").Value,
                ProjectName = xEle.Element("ProjectName").Value,
                ProductNo = xEle.Element("ProductNo").Value,
                ProductDescription = xEle.Element("ProductDescription").Value,
                plannedQty = Convert.ToDecimal(xEle.Element("plannedQty").Value),
                Remarks = xEle.Element("Remarks").Value,
                UOM = xEle.Element("UOM").Value,
                OrderDate = DateTime.ParseExact(xEle.Element("OrderDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),
                DueDate = DateTime.ParseExact(xEle.Element("DueDate").Value, "yyyyMMdd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None),

                Detil = details
            };

            return model;
        }
    }
}