using System;
using System.Collections.Generic;
using System.Text;

namespace Workshop
{
        public class ChinaCalendar
        {
            //Ĭ��ϵͳ��ǰ����
            private DateTime dtvalue = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            //��������ũ���ĳ�ʼ����
            private DateTime baseDate = new DateTime(1900, 1, 31);

            public int chinaYear;       //ũ����
            public int chinaMonth;      //ũ����
            public int doubleMonth;     //����
            public bool isLeap;         //�Ƿ����±��
            public int chinaDay;        //ũ����

            /// <summary>
            /// ��ȡ����������Ӧ������  
            /// </summary>
            public DateTime GetDatetime
            {
                get { return dtvalue; }
                set
                {
                    dtvalue = Convert.ToDateTime(value.ToShortDateString());
                    InitializeValue();
                }
            }

            /// <summary>
            /// ��ȡ��������ࣨ��Ф��
            /// </summary>
            public string GetAnimal
            {
                get { return Animal(); }
            }

            /// <summary>
            ///  ��ȡũ���꣨��� ��֧��
            /// </summary>
            public string GetChinaYear
            {
                get { return ChinaYear(); }
            }

            /// <summary>
            /// ��ȡũ���»�����
            /// </summary>
            public string GetChinaMonth
            {
                get { return ChinaMonth(); }
            }

            /// <summary>
            /// ��ȡũ����
            /// </summary>
            public string GetChinaDay
            {
                get { return ChinaDay(); }
            }

            #region ũ���ľ�̬����

            private static int[] ChinaCalendarInfo = { 0x04bd8,0x04ae0,0x0a570,0x054d5,0x0d260,0x0d950,0x16554,0x056a0,0x09ad0,0x055d2,

                                           0x04ae0,0x0a5b6,0x0a4d0,0x0d250,0x1d255,0x0b540,0x0d6a0,0x0ada2,0x095b0,0x14977,

                                           0x04970,0x0a4b0,0x0b4b5,0x06a50,0x06d40,0x1ab54,0x02b60,0x09570,0x052f2,0x04970,

                                           0x06566,0x0d4a0,0x0ea50,0x06e95,0x05ad0,0x02b60,0x186e3,0x092e0,0x1c8d7,0x0c950,

                                           0x0d4a0,0x1d8a6,0x0b550,0x056a0,0x1a5b4,0x025d0,0x092d0,0x0d2b2,0x0a950,0x0b557,

                                           0x06ca0,0x0b550,0x15355,0x04da0,0x0a5b0,0x14573,0x052b0,0x0a9a8,0x0e950,0x06aa0,

                                           0x0aea6,0x0ab50,0x04b60,0x0aae4,0x0a570,0x05260,0x0f263,0x0d950,0x05b57,0x056a0,

                                           0x096d0,0x04dd5,0x04ad0,0x0a4d0,0x0d4d4,0x0d250,0x0d558,0x0b540,0x0b6a0,0x195a6,

                                           0x095b0,0x049b0,0x0a974,0x0a4b0,0x0b27a,0x06a50,0x06d40,0x0af46,0x0ab60,0x09570,

                                           0x04af5,0x04970,0x064b0,0x074a3,0x0ea50,0x06b58,0x055c0,0x0ab60,0x096d5,0x092e0,

                                           0x0c960,0x0d954,0x0d4a0,0x0da50,0x07552,0x056a0,0x0abb7,0x025d0,0x092d0,0x0cab5,

                                           0x0a950,0x0b4a0,0x0baa4,0x0ad50,0x055d9,0x04ba0,0x0a5b0,0x15176,0x052b0,0x0a930,

                                           0x07954,0x06aa0,0x0ad50,0x05b52,0x04b60,0x0a6e6,0x0a4e0,0x0d260,0x0ea65,0x0d530,

                                           0x05aa0,0x076a3,0x096d0,0x04bd7,0x04ad0,0x0a4d0,0x1d0b6,0x0d250,0x0d520,0x0dd45,

                                           0x0b5a0,0x056d0,0x055b2,0x049b0,0x0a577,0x0a4b0,0x0aa50,0x1b255,0x06d20,0x0ada0,

                                           0x14b63

                                         };

            private static string[] Animals = { "��", "ţ", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

            private static string[] dayStr1 = { "��", "һ", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ" };

            private static string[] dayStr2 = { "��", "ʮ", "إ", "ئ", "��" };

            private static string[] chinaMonthName = { "����", "����", "����", "����", "����", "����", "����", "����", "����", "ʮ��", "ʮһ", "����" };

            private static string[] Gan = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

            private static string[] Zhi = { "��", "��", "��", "î", "��", "��", "��", "δ", "��", "��", "��", "��" };

            //  private static string[] solarTerm = { "С��", "��", "����", "��ˮ", "����", "����", "����", "����", "����", "С��", "â��", "����", "С��", "����", "����", "����", "��¶", "���", "��¶", "˪��", "����", "Сѩ", "��ѩ", "����" };

            private static int[] sTermInfo = { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };

            private static string[] solarTermName ={ 
            "С��", "��", "����", "��ˮ","����", "����", "����", "����","����", "С��", "â��", "����", 
            "С��", "����", "����", "����","��¶", "���", "��¶", "˪��","����", "Сѩ", "��ѩ", "����"};


            #endregion

            #region ���캯��

            public ChinaCalendar()
            { InitializeValue(); }

            public ChinaCalendar(DateTime date)
            {
                dtvalue = Convert.ToDateTime(date.ToShortDateString());
                InitializeValue();
            }

            #endregion

            private void InitializeValue()
            {
                TimeSpan timeSpan = dtvalue - baseDate;
                int sumdays = Convert.ToInt32(timeSpan.TotalDays);   //86400000=1000*24*60*60
                int tempdays = 0;

                //����ũ����
                for (chinaYear = 1900; chinaYear < 2050 && sumdays > 0; chinaYear++)
                {
                    tempdays = ChinaYearDays(chinaYear);
                    sumdays -= tempdays;
                }

                if (sumdays < 0)
                {
                    sumdays += tempdays;
                    chinaYear--;
                }

                //��������
                doubleMonth = DoubleMonth(chinaYear);
                isLeap = false;

                //����ũ����
                for (chinaMonth = 1; chinaMonth < 13 && sumdays > 0; chinaMonth++)
                {
                    //����
                    if (doubleMonth > 0 && chinaMonth == (doubleMonth + 1) && isLeap == false)
                    {
                        --chinaMonth;
                        isLeap = true;
                        tempdays = DoubleMonthDays(chinaYear);
                    }
                    else
                    {
                        tempdays = MonthDays(chinaYear, chinaMonth);
                    }

                    //�������
                    if (isLeap == true && chinaMonth == (doubleMonth + 1))
                    {
                        isLeap = false;
                    }
                    sumdays -= tempdays;
                }

                //����ũ����
                if (sumdays == 0 && doubleMonth > 0 && chinaMonth == doubleMonth + 1)
                {
                    if (isLeap)
                    {
                        isLeap = false;
                    }
                    else
                    {
                        isLeap = true;
                        --chinaMonth;
                    }
                }

                if (sumdays < 0)
                {
                    sumdays += tempdays;
                    --chinaMonth;
                }

                chinaDay = sumdays + 1;

                //�������
                ComputeSolarTerm();
            }

            ///<summary>
            ///����ũ�����������
            ///</summary>
            ///<param name="year">ũ����</param>
            ///<returns></returns>
            private int ChinaYearDays(int year)
            {
                int i, sum = 348;
                for (i = 0x8000; i > 0x8; i >>= 1)
                {
                    sum += ((ChinaCalendarInfo[year - 1900] & i) != 0) ? 1 : 0;
                }
                return (sum + DoubleMonthDays(year));
            }

            ///<summary>
            ///����ũ���������·�1-12 , û�򷵻�0
            ///</summary>
            ///<param name="year">ũ����</param>
            ///<returns></returns>
            private int DoubleMonth(int year)
            {
                return (ChinaCalendarInfo[year - 1900] & 0xf);
            }

            ///<summary>
            ///����ũ�������µ�����
            ///</summary>
            ///<param name="year">ũ����</param>
            ///<returns></returns>
            private int DoubleMonthDays(int year)
            {
                if (DoubleMonth(year) != 0)
                    return (((ChinaCalendarInfo[year - 1900] & 0x10000) != 0) ? 30 : 29);
                else
                    return (0);
            }

            ///</summary>
            ///����ũ�����·ݵ�������
            ///</summary>
            ///<param name="year">ũ����</param>
            ///<param name="month">ũ����</param>
            ///<returns></returns>
            private int MonthDays(int year, int month)
            {
                return (((ChinaCalendarInfo[year - 1900] & (0x10000 >> month)) != 0) ? 30 : 29);
            }

            //�������ࣨ��Ф��
            private string Animal()
            {
                return Animals[(chinaYear - 4) % 60 % 12];
            }

            //����ũ�����ַ���
            public string ChinaYear()
            {
                return (Gan[(chinaYear - 4) % 60 % 10] + Zhi[(chinaYear - 4) % 60 % 12] + "��");
            }

            //����ũ�����ַ���
            private string ChinaMonth()
            {
                if (isLeap == true)
                {
                    return "��" + chinaMonthName[chinaMonth - 1];
                }
                else
                {
                    return chinaMonthName[chinaMonth - 1];
                }
            }

            //����ũ�����ַ���
            private string ChinaDay()
            {
                string s;
                switch (chinaDay)
                {
                    case 10:
                        s = "��ʮ";
                        break;
                    case 20:
                        s = "��ʮ";
                        break;
                    case 30:
                        s = "��ʮ";
                        break;
                    default:
                        s = dayStr2[chinaDay / 10];
                        s += dayStr1[chinaDay % 10];
                        break;
                }
                return (s);
            }

            #region ����

            public struct SolarTerm
            {
                /// <summary>
                /// ����������
                /// </summary>
                public string Name;

                /// <summary>
                /// ������ʱ��
                /// </summary>
                public DateTime DateTime;
            }

            private SolarTerm[] solarTerm = new SolarTerm[2];

            /// <summary>
            /// ����ָ�����ڵ��·��������������Ƽ�ʱ���SolarTerm����
            /// </summary>
            public SolarTerm[] GetSolarTerm
            {
                get
                {
                    return solarTerm;
                }
            }

            /// <summary>
            /// ����ָ�����ڵĽ�����,û�н������򷵻ؿ��ַ���
            /// </summary>
            public string GetTermName
            {
                get
                {
                    foreach (SolarTerm sterm in solarTerm)
                        if (dtvalue.Day == sterm.DateTime.Day)
                        {
                            return sterm.Name;
                        }

                    return "";
                }
            }

            // �������
            private void ComputeSolarTerm()
            {
                int year = dtvalue.Year;
                int month = dtvalue.Month;

                for (int n = month * 2 - 1; n <= month * 2; n++)
                {
                    double Termdays = Term(year, n, true);
                    double mdays = AntiDayDifference(year, Math.Floor(Termdays));
                    double sm1 = Math.Floor(mdays / 100);
                    int hour = (int)Math.Floor((double)Tail(Termdays) * 24);
                    int minute = (int)Math.Floor((double)(Tail(Termdays) * 24 - hour) * 60);
                    int tMonth = (int)Math.Ceiling((double)n / 2);
                    int day = (int)mdays % 100;

                    solarTerm[n - month * 2 + 1].Name = solarTermName[n - 1];
                    solarTerm[n - month * 2 + 1].DateTime = new DateTime(year, tMonth, day, hour, minute, 0);
                }
            }


            //����y���n����������С��Ϊ1�����ղ�����ֵ��pdȡֵ��٣��ֱ��ʾƽ���Ͷ�����
            private double Term(int y, int n, bool pd)
            {
                //������
                double juD = y * (365.2423112 - 6.4e-14 * (y - 100) * (y - 100) - 3.047e-8 * (y - 100)) + 15.218427 * n + 1721050.71301;

                //�Ƕ�
                double tht = 3e-4 * y - 0.372781384 - 0.2617913325 * n;

                //���ʵ����
                double yrD = (1.945 * Math.Sin(tht) - 0.01206 * Math.Sin(2 * tht)) * (1.048994 - 2.583e-5 * y);

                //˷��ʵ����
                double shuoD = -18e-4 * Math.Sin(2.313908653 * y - 0.439822951 - 3.0443 * n);

                double vs = (pd) ? (juD + yrD + shuoD - EquivalentStandardDay(y, 1, 0) - 1721425) : (juD - EquivalentStandardDay(y, 1, 0) - 1721425);
                return vs;
            }


            // ��������y���ղ�����Ϊxʱ����Ӧ������������y=2000��x=274ʱ������1001(��ʾ10��1�գ�������100*m+d)��
            private double AntiDayDifference(int y, double x)
            {
                int m = 1;
                for (int j = 1; j <= 12; j++)
                {
                    int mL = DayDifference(y, j + 1, 1) - DayDifference(y, j, 1);
                    if (x <= mL || j == 12)
                    {
                        m = j;
                        break;
                    }
                    else
                        x -= mL;
                }
                return 100 * m + x;
            }


            // ����x��С��β������xΪ��ֵ������1-С��β��
            private double Tail(double x)
            {
                return x - Math.Floor(x);
            }


            // ���ص�Ч��׼������y��m��d����Ӧ���ֵ�1��1��1�յĵ�Ч(����Gregorian����Julian����ͳһ��)������
            private double EquivalentStandardDay(int y, int m, int d)
            {
                //Julian�ĵ�Ч��׼����
                double v = (y - 1) * 365 + Math.Floor((double)((y - 1) / 4)) + DayDifference(y, m, d) - 2;

                if (y > 1582)
                {//Gregorian�ĵ�Ч��׼����
                    v += -Math.Floor((double)((y - 1) / 100)) + Math.Floor((double)((y - 1) / 400)) + 2;
                }
                return v;
            }


            // ��������y��m��d�յ��ղ���������y���������߹�����������2000��3��1��Ϊ61��
            private int DayDifference(int y, int m, int d)
            {
                int ifG = IfGregorian(y, m, d, 1);
                int[] monL = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                if (ifG == 1)
                    if ((y % 100 != 0 && y % 4 == 0) || (y % 400 == 0))
                        monL[2] += 1;
                    else
                        if (y % 4 == 0)
                            monL[2] += 1;
                int v = 0;
                for (int i = 0; i <= m - 1; i++)
                {
                    v += monL[i];
                }
                v += d;
                if (y == 1582)
                {
                    if (ifG == 1)
                        v -= 10;
                    if (ifG == -1)
                        v = 0;  //infinity 
                }
                return v;
            }


            // �ж�y��m��(1,2,..,12,��ͬ)d����Gregorian������Julian��
            //��opt=1,2,3�ֱ��ʾ��׼����,Gregorge����Julian����,���򷵻�1����Julian���򷵻�0��
            // ����Gregorge����ɾȥ����10���򷵻�-1
            private int IfGregorian(int y, int m, int d, int opt)
            {
                if (opt == 1)
                {
                    if (y > 1582 || (y == 1582 && m > 10) || (y == 1582 && m == 10 && d > 14))
                        return (1);     //Gregorian
                    else
                        if (y == 1582 && m == 10 && d >= 5 && d <= 14)
                            return (-1);  //��
                        else
                            return (0);  //Julian
                }

                if (opt == 2)
                    return (1);     //Gregorian
                if (opt == 3)
                    return (0);     //Julian
                return (-1);
            }

            #endregion ����
        }
}
