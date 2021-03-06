﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;   //open text file

namespace SinoTunnel  // 網路資料庫資料變數定義
{
    //public class userGlobal
    //{
    //    public static string projectUID = "";
    //    public static string projectTitle = "";
    //    public static string SectionUID = "";
    //}

    public struct TunnelBoringData  //定義潛盾機基本構件資料
    {
        public string UID;      //  ID
        public string Sectioon; //  Section ID    
        public float DO;        //  潛盾機外徑(m)
        public float L;         //	潛盾機長度(m)
        public float W;         //	潛盾機重量(T)
        public float DI;        //	潛盾機內徑(m)
        public float LF;        //	盾首前緣突出部分(m)
        public int NS;        //	與盾尾止水封接觸環 1片數(環)
        public float WS;        //	單環環片重量(T)
        public float U1;        //	盾殼與土壤間之摩擦係數
        public float U2;        //	0.3	環片與盾尾止水封之摩擦係數
        public float D;         //	同步背填灌漿管直徑(m)
        public float LD;        //  同步背填灌漿管長度(m)
    }

    public struct SoilLayer         //STN_SoilLayer  //定義土層變數
    {
        public string UID;          //  ID
        public string Sectioon;     //  Section ID    
        public string SoilType;     //  土層分類
        public float Depth;         //	土層深度(m)
        public float γ;            //	單位重(kN/m³)(T)
        public float N;             //	潛盾機內徑(m)
        public float Suδv;         //	Suδv盾首前緣突出部分(m)
        public float ν;            //	ν柏松比
        public float φ;            //	φ摩擦角(°)度
        public DateTime CreateDate; //	建立日期
        public string CreateUser;   //建立者
    }

    public struct Load              //STN_Load //定義建物及交通載重變數
    {
        public string UID;          //  ID
        public string Sectioon;     //  Section ID    
        public float P;            //  載重值(kN/m²)
        public float X1;            //	X1(m)
        public float X2;            //	X2(m)
        public DateTime CreateDate; //	建立日期
        public string CreateUser;   //  建立者
    }

    public struct Coord              
    {
        public string Joint;        // Joint
        public string CoordSys;     // CoordSys
        public string CoordType;    // CoordType
        public double XorR;         // XorR (m)        
        public double Y;            // Y (m)
        public double T;            // T / Degree  (m)
        public double Z;            // Z (m)
        public double θ;           // 環片夾角(度)
    }

    //public struct Segment //環片參數
    //{
    //    public string UID;
    //    public string Section;
    //    public string Name;
    //    public string MaterialUID;
    //    public string Shape;
    //    public double Width;
    //    public double Height;
    //}

    //public struct Material 
    //{
    //    public string UID;
    //    public string ProjectUID;
    //    public string Name;
    //    public string Type;
    //    public double UnitWeight;
    //    public double YoungsModulus;
    //    public double PoissonRatio;
    //    public double CTE;
    //    public double Fc;
    //}

    public struct SGMainSteel
    {
        public double SGYBarNo1;
        public double SGYBarNo2;
        public int SGYBarNum;
        public double SGYBarArea1;
        public double SGYBarArea2;
        public double Area;
    }

    public struct Connector
    {
        public string LoadType;
        public float TR;
        public float BH;
        public double CoverDepth;
        public double UnitWeight;
        public double PoissonRatio;
        public double Fc;
        public double Fy;
        public double E;        
    }

    public struct Steel
    {
        public string Material;
        public double UW;
        public double U12;
        public double Fy;
        public double Fu;
        public double CutB;
        
    }

    class SG
    {
        public List<double> angle;
        public List<double> X;
        public List<double> Y;
        public List<double> Z;
        public SG()
        {
            angle = new List<double>();
            X = new List<double>();
            Y = new List<double>();
            Z = new List<double>();
        }
    }

    class SGAngle
    {
        public double BK_Angle;
        public double KB_Angle;
        public double BA_Angle;
        public double AB_Angle;
        public SGAngle()
        {
            BK_Angle = 0;
            KB_Angle = 0;
            BA_Angle = 0;
            AB_Angle = 0;
        }
    }

}
