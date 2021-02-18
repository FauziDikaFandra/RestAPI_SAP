using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDU_REST.Logic;

namespace IDU_REST.Models
{
    #region "Return"
    public abstract class RTNVAL
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public int recordCount { get; set; }
    }
    #endregion
    #region "Release"
    public class RELVAL : RTNVAL
    {
        public List<REL> values { get; set; }
    }

    public class RELSKALARVAL : RTNVAL
    {
        public REL values { get; set; }
    }
    #endregion

    #region "AR Invoice"
    public class ARINVOICEVAL : RTNVAL
    {
        public List<ARINV_HEADER> values { get; set; }
    }

    public class ARINVOICESKALARVAL : RTNVAL
    {
        public ARINV_HEADER values { get; set; }
    }
    #endregion

    #region "Purchase Order"
    public class POVAL : RTNVAL
    {
        public List<PO_HEADER> values { get; set; }
    }

    public class POSKALARVAL : RTNVAL
    {
        public PO_HEADER values { get; set; }
    }
    #endregion

    #region "Goods Receipt Purchase Order"
    public class GRPOVAL : RTNVAL
    {
        public List<GRPO_HEADER> values { get; set; }
    }
  
    public class GRPOSKALARVAL : RTNVAL
    {
        public GRPO_HEADER values { get; set; }
    }
    public class GRPOSKALARVALLIST : RTNVAL
    {
        public List<GRPO_HEADER> values { get; set; }
    }
    public class GetListStockVal : RTNVAL
    {
        public List<GetListStock> Value { get; set; }
    }
    #endregion

    #region "Inventory Transfer"
    public class ITVAL : RTNVAL
    {
        public List<IT_HEADER> values { get; set; }
    }

    public class ITSKALARVAL : RTNVAL
    {
        public IT_HEADER values { get; set; }
        public List<IT_HEADER> valueList { get; set; }
        public int DocEntry { get; set; }
        public string Status { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public int RecordCount { get; set; }
    }
    #endregion

    #region "Purchase Order"
    public class GRVAL : RTNVAL
    {
        public List<GR_HEADER> values { get; set; }
    }

    public class GRSKALARVAL : RTNVAL
    {
        public GR_HEADER values { get; set; }
    }
    #endregion

    #region "Business Partner"
    public class BPVAL : RTNVAL
    {
        public List<BP> values { get; set; }
    }
    public class EXRVAL : RTNVAL
    {
        public List<EXR> values { get; set; }
    }
    public class BPSKALARVAL : RTNVAL
    {
        public BP values { get; set; }
    }
    #endregion

    #region "Item Master"
    public class ITEMVAL : RTNVAL
    {
        public List<ITEM> values { get; set; }
    }
    
    public class ITEMSKALARVAL : RTNVAL
    {
        public ITEM values { get; set; }
    }
    #endregion

    #region "Sales Order"
    public class SOSKALAR : RTNVAL
    {
        public List<SO_HEADER> value { get; set; }
    }
    public class SOSKALARVAL : RTNVAL
    {
        public SO_HEADER values { get; set; }
    }
    #endregion
    #region "Good Issue"
    public class GISOSKALAR : RTNVAL
    {
        public List<GI_HEADER> value { get; set; }
    }
    public class GISKALARVAL : RTNVAL
    {
        public GI_HEADER values { get; set; }
    }
    #endregion


    #region "Delivery Order"
    
    public class DOSOSKALAR : RTNVAL
    {
        public List<DO_HEADER> value { get; set; }
    }
    public class DOSKALARVAL : RTNVAL
    {
        public DO_HEADER values { get; set; }
        public List<DO_HEADER> lstValue { get; set; }
    }
    #endregion

    #region "Manipulate Data"
    public class RTNMANVAL
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string value { get; set; }
        
        
        public int DocEntry { get; set; }
        public string Status { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public int RecordCount { get; set; }
    }

    public class RTITR
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string value { get; set; }
        public List<ITR> valueList { get; set; }
        public int DocEntry { get; set; }
        public string Status { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public int RecordCount { get; set; }
         
    }


    public class RTGIPROD  
    {

        public string errorCode { get; set; }
        public string message { get; set; }
        public string value { get; set; }
        public List<PROD> valueLis { get; set; }
        public string NumberForm { get; set; }
        public string OrderNumber { get; set; }
        public string SeriesNo {get;set;}
        public string ItemNo { get; set; }
        public string ItemDescription { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal ItemCost { get; set; }
        public int RecordCount { get; set; }
    }
    #endregion
}