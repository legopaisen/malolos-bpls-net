using System;
using System.Collections.Generic;
using System.Text;



namespace Amellar.Common.BPLSApp
{
    public class BPLSAppSetting
    {
        // for own_names
        public string m_sOwnName, m_sOwnLn, m_sOwnFn, m_sOwnMi, m_sOwnHouseNo, m_sOwnStreet, m_sOwnDist,
            m_sOwnZone, m_sOwnBrgy, m_sOwnMun, m_sOwnProv, m_sOwnZip, m_sOwnerCode; // RMC 20110414 added owner code

        // for businesses
        public string m_sBIN, m_sBNS_NM, m_sBNS_STAT, m_sOWN_CODE, m_sBNS_TELNO, m_sBNS_HOUSE_NO, m_sBNS_STREET,
           m_sBNS_MUN, m_sBNS_DIST, m_sBNS_ZONE, m_sBNS_BRGY, m_sBNS_PROV, m_sBNS_ZIP, m_sLAND_PIN,
           m_sBLDG_PIN, m_sMACH_PIN, m_sPOFF_HOUSE_NO, m_sPOFF_STREET, m_sPOFF_MUN, m_sPOFF_DIST,
           m_sPOFF_ZONE, m_sPOFF_BRGY, m_sPOFF_PROV, m_sPOFF_ZIP, m_sORGN_KIND, m_sBUSN_OWN, m_sCTC_NO,
           m_sCTC_ISSUED_AT, m_sBNS_CODE, m_sSSS_NO, m_sDTI_REG_NO,
           m_sBLDG_VAL, m_sPLACE_OCCUPANCY, m_sRENT_LEASE_MO,
           m_sFLR_AREA, m_sNUM_STOREYS, m_sTOT_FLR_AREA, m_sPREV_BNS_OWN, m_sNUM_EMPLOYEES, m_sNUM_PROFESSIONAL,
           m_sANNUAL_WAGES, m_sAVE_ELECTRIC_BILL, m_sAVE_WATER_BILL, m_sAVE_PHONE_BILL, m_sOTHER_UTIL,
           m_sNUM_DELIV_VEHICLE, m_sNUM_MACHINERIES, m_sPERMIT_NO, m_sPERMIT_DT,
           m_sGR_1, m_sGR_2, m_sCAPITAL, m_sOR_NO, m_sTAX_YEAR, m_sCANC_REASON, m_sCANC_BY,
           m_sBNS_USER, m_sMEMORANDA, m_sCTC_ISSUED_ON, m_sSSS_ISSUED_ON, m_sDTI_REG_DT, m_sRENT_LEASE_SINCE, m_sDT_OPERATED, m_sCANC_DATE, m_sSAVE_TM,
           m_sEmail, m_sTIN, m_sTINIssuedOn, m_sDateApplied;   // RMC 20110803

       // GDE 20090212 for businesses
        public BPLSAppSetting(string sBnsNm, string sBnsStat, string sOwnCode, string sBnsTelNo, string sBnsHouseNo, string sBnsStreet,
            string sBnsMun, string sBnsDist, string sBnsZone, string sBnsBrgy, string sBnsProv, string sBnsZip, string sLandPin,
            string sBldgPin, string sMachPin, string sPoffHouseNo, string sPoffStreet, string sPoffMun, string sPoffDist,
            string sPoffZone, string sPoffbrgy, string sPoffProv, string sPoffZip, string sOrgnKind, string sBussOwn, string sCTCCno,
            string sCTCIssuedOn, string sCTCIssuedAt, string sBnsCode, string sSSSNo, string sSSSIssuedOn, string sDTIRegNo, string sDTIRegDate,
            double dBldgVal, string sPlaceOccupancy, string sRentLeaseSince, double dRentLeaseMo,
            double dFlrArea, string sNumStorey, double dTotFlrArea, string sPrevBnsOwn, string sNumEmployees, string sNumProfessional,
            double dAnnualWages, double dAveElectricBill, double dAveWaterBill, double dAvePhoneBill, double dOtherUtil,
            string sNumDelivVehicle, string sNumMachineries, string sDTOperated, string sPermitNo, string sPermitDate,
            double dGr1, double dGr2, double dCapital, string sOrNo, string sTaxYear, string sCancReason, string sCancDate, string sCancBy,
            string sBnsUser, string sSaveTime, string sMemo,
            string sEmail, string sTIN, string sTINIssuedOn, string sDateApplied)   // RMC 20110803 
        {
            // RMC 20110311 changed data type for ff: variables sBldgVal, sRentLeaseMo, sFlrArea, sTotFlrArea,
            //sAnnualWages,sAveElectricBill, sAveWaterBill, sAvePhoneBill, sOtherUtil, sGr1, sGr2, sCapital
            
           // m_sBIN                   = sBIN;
            m_sBNS_NM                = sBnsNm;
            m_sBNS_STAT              = sBnsStat;
            m_sOWN_CODE              = sOwnCode;
            m_sBNS_TELNO             = sBnsTelNo;
            m_sBNS_HOUSE_NO          = sBnsHouseNo;
            m_sBNS_STREET            = sBnsStreet;
            m_sBNS_MUN               = sBnsMun;
            m_sBNS_DIST              = sBnsDist;
            m_sBNS_ZONE              = sBnsZone;
            m_sBNS_BRGY              = sBnsBrgy;
            m_sBNS_PROV              = sBnsProv;
            m_sBNS_ZIP               = sBnsZip;
            m_sLAND_PIN              = sLandPin;
            m_sBLDG_PIN              = sBldgPin;
            m_sMACH_PIN              = sMachPin;
            m_sPOFF_HOUSE_NO         = sPoffHouseNo;
            m_sPOFF_STREET           = sPoffStreet;
            m_sPOFF_MUN              = sPoffMun;
            m_sPOFF_DIST             = sPoffDist;
            m_sPOFF_ZONE             = sPoffZone;
            m_sPOFF_BRGY             = sPoffbrgy;
            m_sPOFF_PROV             = sPoffProv;
            m_sPOFF_ZIP              = sPoffZip;
            m_sORGN_KIND             = sOrgnKind;
            m_sBUSN_OWN              = sBussOwn; 
            m_sCTC_NO                = sCTCCno;
            m_sCTC_ISSUED_ON         = sCTCIssuedOn;
            m_sCTC_ISSUED_AT         = sCTCIssuedAt;
            m_sBNS_CODE              = sBnsCode;
            m_sSSS_NO                = sSSSNo;
            m_sSSS_ISSUED_ON         = sSSSIssuedOn;
            m_sDTI_REG_NO            = sDTIRegNo;
            m_sDTI_REG_DT            = sDTIRegDate;
            m_sBLDG_VAL = string.Format("{0:##.00}", dBldgVal);    // RMC 20110311 changed data type
            m_sPLACE_OCCUPANCY       = sPlaceOccupancy;
            // RMC 20141217 adjustments (s)
            if (sPlaceOccupancy == "OWNED")
                sRentLeaseSince = "";
            // RMC 20141217 adjustments (e)

            m_sRENT_LEASE_SINCE      = sRentLeaseSince;
            m_sRENT_LEASE_MO = string.Format("{0:##.00}", dRentLeaseMo);    // RMC 20110311 changed data type
            m_sFLR_AREA = string.Format("{0:##.00}", dFlrArea);    // RMC 20110311 changed data type
            m_sNUM_STOREYS           = sNumStorey;
            m_sTOT_FLR_AREA = string.Format("{0:##.00}", dTotFlrArea); // RMC 20110311 changed data type
            m_sPREV_BNS_OWN          = sPrevBnsOwn;
            m_sNUM_EMPLOYEES         = sNumEmployees;
            m_sNUM_PROFESSIONAL      = sNumProfessional;
            m_sANNUAL_WAGES = string.Format("{0:##.00}", dAnnualWages);    // RMC 20110311 changed data type
            m_sAVE_ELECTRIC_BILL = string.Format("{0:##.00}", dAveElectricBill);    // RMC 20110311 changed data type
            m_sAVE_WATER_BILL = string.Format("{0:##.00}", dAveWaterBill);   // RMC 20110311 changed data type
            m_sAVE_PHONE_BILL = string.Format("{0:##.00}", dAvePhoneBill);   // RMC 20110311 changed data type
            m_sOTHER_UTIL = string.Format("{0:##.00}", dOtherUtil);  // RMC 20110311 changed data type
            m_sNUM_DELIV_VEHICLE     = sNumDelivVehicle;
            m_sNUM_MACHINERIES       = sNumMachineries;
            m_sDT_OPERATED           = sDTOperated;
            m_sPERMIT_NO             = sPermitNo;
            m_sPERMIT_DT             = sPermitDate; // GDE 20110201
            m_sGR_1 = string.Format("{0:##.00}", dGr1);    // RMC 20110311 changed data type
            m_sGR_2 = string.Format("{0:##.00}", dGr2);    // RMC 20110311 changed data type
            m_sCAPITAL = string.Format("{0:##.00}", dCapital);    // RMC 20110311 changed data type
            m_sOR_NO                 = sOrNo;
            m_sTAX_YEAR              = sTaxYear;
            m_sCANC_REASON           = sCancReason;
            m_sCANC_DATE             = sCancDate;
            m_sCANC_BY               = sCancBy;
            m_sBNS_USER              = sBnsUser;
            m_sSAVE_TM               = sSaveTime;
            m_sMEMORANDA             = sMemo;
            m_sEmail = sEmail;  // RMC 20110803
            m_sTIN = sTIN;  // RMC 20110803
            m_sTINIssuedOn = sTINIssuedOn;  // RMC 20110803
            m_sDateApplied = sDateApplied;  // RMC 20110803
         }

         // GDE 20090212 for own_names
        public BPLSAppSetting(string sLn, string sFn, string sMi, string sOwnHouseNo,
            string sOwnStreet, string sOwnDist, string sOwnZone, string sOwnBrgy, string sOwnMun,
            string sOwnProv, string sOwnZip, string sOwnerCode) // RMC 20110414 added OwnerCode
        {
            m_sOwnLn = sLn;
            m_sOwnFn = sFn;
            m_sOwnMi = sMi;
            m_sOwnHouseNo = sOwnHouseNo;
            m_sOwnStreet = sOwnStreet;
            m_sOwnDist = sOwnDist;
            m_sOwnZone = sOwnZone;
            m_sOwnBrgy = sOwnBrgy;
            m_sOwnMun = sOwnMun;
            m_sOwnProv = sOwnProv;
            m_sOwnZip = sOwnZip;
            m_sOwnerCode = sOwnerCode;  // RMC 20110414
        }

        // GDE 20090212 OWN_NAMES (s){
        public string sOwnProv
        {
            get { return m_sOwnProv; }
        }
        public string sOwnZip
        {
            get { return m_sOwnZip; }
        }
        public string sOwnZone
        {
            get { return m_sOwnZone; }
        }
        public string sOwnBrgy
        {
            get { return m_sOwnBrgy; }
        }
        public string sOwnMun
        {
            get { return m_sOwnMun; }
        }
        public string sOwnHouseNo
        {
            get { return m_sOwnHouseNo; }
        }
        public string sOwnStreet
        {
            get { return m_sOwnStreet; }
        }
        public string sOwnDist
        {
            get { return m_sOwnDist; }
        }
        public string sLn
        {
            get { return m_sOwnLn; }
        }
        public string sFn
        {
            get { return m_sOwnFn; }
        }
        public string sMi
        {
            get { return m_sOwnMi; }
        }
        public string sOwnerCode    // RMC 20110414 added owner code
        {
            get { return m_sOwnerCode; }
        }

        // GDE 20090212 OWN_NAMES (e)}

        // GDE 20090212 BUSINESSES (

        

        public string sCTCIssuedOn
        {
            get { return m_sCTC_ISSUED_ON; }
        }
        public string sSSSIssuedOn
        {
            get { return m_sSSS_ISSUED_ON; }
        }
        public string sDTIRegDate
        {
            get { return m_sDTI_REG_DT; }
        }
        public string sRentLeaseSince
        {
            get { return m_sRENT_LEASE_SINCE; }
        }
        public string sDTOperated
        {
            get { return m_sDT_OPERATED; }
        }
        public string sCancDate
        {
            get { return m_sCANC_DATE; }
        }
        public string sSaveTime
        {
            get { return m_sSAVE_TM; }
        }

        public string sBnsProv
        {
            get { return m_sBNS_PROV; }
        }
        public string sMemo
        {
            get { return m_sMEMORANDA; }
        }
        public string sBnsUser
        {
            get { return m_sBNS_USER; }
        }
        public string sCancBy
        {
            get { return m_sCANC_BY; }
        }
        public string sCancReason
        {
            get { return m_sCANC_REASON; }
        }
        public string sTaxYear
        {
            get { return m_sTAX_YEAR; }
        }
        public string sOrNo
        {
            get { return m_sOR_NO; }
        }
        public string sCapital
        {
            get { return m_sCAPITAL; }
        }
        public string sGr2
        {
            get { return m_sGR_2; }
        }
        public string sGr1
        {
            get { return m_sGR_1; }
        }
        public string sPermitNo
        {
            get { return m_sPERMIT_NO; }
        }
        public string sPermitDate
        {
            get { return m_sPERMIT_DT; }
        }
        public string sNumMachineries
        {
            get { return m_sNUM_MACHINERIES; }
        }
        public string sNumDelivVehicle
        {
            get { return m_sNUM_DELIV_VEHICLE; }
        }
        public string sOtherUtil
        {
            get { return m_sOTHER_UTIL; }
        }
        public string sAvePhoneBill
        {
            get { return m_sAVE_PHONE_BILL; }
        }
        public string sAveWaterBill
        {
            get { return m_sAVE_WATER_BILL; }
        }
        public string sAveElectricBill
        {
            get { return m_sAVE_ELECTRIC_BILL; }
        }
        public string sAnnualWages
        {
            get { return m_sANNUAL_WAGES; }
        }
        public string sNumProfessional
        {
            get { return m_sNUM_PROFESSIONAL; }
        }
        public string sNumEmployees
        {
            get { return m_sNUM_EMPLOYEES; }
        }
        public string sPrevBnsOwn
        {
            get { return m_sPREV_BNS_OWN; }
        }
        public string sTotFlrArea
        {
            get { return m_sTOT_FLR_AREA; }
        }
        public string sNumStorey
        {
            get { return m_sNUM_STOREYS; }
        }
        public string sFlrArea
        {
            get { return m_sFLR_AREA; }
        }
        public string sRentLeaseMo
        {
            get { return m_sRENT_LEASE_MO; }
        }
        public string sPlaceOccupancy
        {
            get { return m_sPLACE_OCCUPANCY; }
        }
        public string sBldgVal
        {
            get { return m_sBLDG_VAL; }
        }
        public string sDTIRegNo
        {
            get { return m_sDTI_REG_NO; }
        }
        public string sSSSNo
        {
            get { return m_sSSS_NO; }
        }
        public string sBnsCode
        {
            get { return m_sBNS_CODE; }
        }
        public string sCTCIssuedAt
        {
            get { return m_sCTC_ISSUED_AT; }
        }
        public string sCTCCno
        {
            get { return m_sCTC_NO; }
        }
        public string sBussOwn
        {
            get { return m_sBUSN_OWN; }
        }
        public string sOrgnKind
        {
            get { return m_sORGN_KIND; }
        }
        public string sPoffZip
        {
            get { return m_sPOFF_ZIP; }
        }
        public string sPoffProv
        {
            get { return m_sPOFF_PROV; }
        }
        public string sPoffbrgy
        {
            get { return m_sPOFF_BRGY; }
        }
        public string sPoffZone
        {
            get { return m_sPOFF_ZONE; }
        }
        public string sLandPin
        {
            get { return m_sLAND_PIN; }
        }
        public string sBnsZip
        {
            get { return m_sBNS_ZIP; }
        }
        public string aBnsProv
        {
            get { return m_sBNS_PROV; }
        }
        public string sBnsBrgy
        {
            get { return m_sBNS_BRGY; }
        }
        public string sBnsZone
        {
            get { return m_sBNS_ZONE; }
        }
        public string sBnsDist
        {
            get { return m_sBNS_DIST; }
        }
        public string sBnsMun
        {
            get { return m_sBNS_MUN; }
        }
        public string sPoffDist
        {
            get { return m_sPOFF_DIST; }
        }
        public string sPoffMun
        {
            get { return m_sPOFF_MUN; }
        }
        public string sBldgPin
        {
            get { return m_sBLDG_PIN; }
        }
        public string sMachPin
        {
            get { return m_sMACH_PIN; }
        }
        public string sPoffHouseNo
        {
            get { return m_sPOFF_HOUSE_NO; }
        }
        public string sPoffStreet
        {
            get { return m_sPOFF_STREET; }
        }
        public string sBnsStreet
        {
            get { return m_sBNS_STREET; }
        }
        public string sBnsHouseNo
        {
            get { return m_sBNS_HOUSE_NO; }
        }
        public string sBnsTelNo
        {
            get { return m_sBNS_TELNO; }
        }
        public string sOwnCode
        {
            get { return m_sOWN_CODE; }
        }
        public string sBnsStat
        {
            get { return m_sBNS_STAT; }
        }
        public string sBIN
        {
            get { return m_sBIN; }
            
        }
        public string sBnsNm
        {
            get { return m_sBNS_NM; }
        }
        // GDE 20090212 BUSINESSES (e)}

        public BPLSAppSetting()
        {
            this.Clear();
        }

        public void Clear()
        {
            m_sBIN = string.Empty;
        }


    }
}
