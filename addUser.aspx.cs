using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
public partial class editUser : basePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            usrActive.Checked = true;  //设置默认值
            //读取权限列表
            string strSql11 = "select * from lpUserPriority order by lpIndex ";
            DataTable o = tinyDB.SearchData2objTable(strSql11);
            foreach(DataRow r in o.Rows)
            {
                CheckBoxList1.Items.Add(r["lpPriorityName"].ToString());               
            }
            if (CheckBoxList1.Items.Count >= 0) CheckBoxList1.Items[0].Selected = true;   //设置默认值
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        bool judge = true;
        //检验用户名是否重复
        string strSql1 = "select count(*) from lpUserAccount where usrLoginName=@usrLoginName and usrIsDelete = 0";
        SqlParameterCollection myParas = tinyDB.InitParameterCollection();
        myParas.AddWithValue("@usrLoginName", usrLoginName.Text.Trim()); 
        if( tinyDB.gettheLongFromDB (strSql1, myParas)>0)
        {
            Msg("该登录名已经存在，请输入其它登录名");
            judge = false;
            return;
        }
        myParas.Clear();
        //检验用户工号是否重复
        string strSql2 = "select count(*) from lpUserAccount where usrWorkNo=@usrWorkNo and usrIsDelete = 0";
        myParas.AddWithValue("@usrWorkNo", usrWorkNo.Text.Trim());
        if (tinyDB.gettheLongFromDB(strSql2, myParas) > 0)
        {
            Msg("该用户工号已经存在，请输入其它工号");
            judge = false;
            return;
        }

        

        if (judge)
        {
            try
            {
                //添加用户基本信息
                string ePassword = tinyUtil.MD5Encrypt(usrLoginPassword.Text.ToString());
                int active;
                if (usrActive.Checked == true) { active = 1; }
                else { active = 0; }
                string strSql3 = "insert into lpUserAccount (usrLoginName,usrLoginPassword,usrActive,usrRealName,usrWorkNo)values('" + usrLoginName.Text.ToString().Trim() + "','" + ePassword + "','" + active + "','" + usrRealName.Text.ToString().Trim() + "','" + usrWorkNo.Text.ToString().Trim() + "')";
                SqlParameterCollection myParas1 = tinyDB.InitParameterCollection();
                tinyDB.Update2DB(strSql3, myParas1).ToString();   //写到数据库
                myParas1.Clear();
                insertRecord("lpUserAccount");//插入记录

                //添加用户权限
                string strSql4 = "select usrID from lpUserAccount where usrWorkNo=@usrWorkNo";
                myParas1.AddWithValue("@usrWorkNo", usrWorkNo.Text.ToString());
                DataTable c = tinyDB.SearchData2objTable(strSql4, myParas1);
                Int64 usrID = Convert.ToInt64(c.Rows[0][0].ToString());
                myParas1.Clear();
                string strSql11 = "select * from lpUserPriority ";
                DataTable oo = tinyDB.SearchData2objTable(strSql11);
                for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                {
                    if (CheckBoxList1.Items[i].Selected == true)
                    {
                        foreach(DataRow r in oo.Rows)
                        {
                            if (CheckBoxList1.Items[i].Text == r["lpPriorityName"].ToString())
                            {
                                string strSql5 = "insert into lpUser2Priority (usrID,lpID)values('" + usrID + "','" + r["lpID"]+ "')";
                                tinyDB.Update2DB(strSql5, myParas1).ToString();   //写到数据库
                                break;
                            }
                        }
                    }
                }
                insertRecord("lpUser2Priority");//插入记录
                oo.Dispose();

                Response.Redirect("manage_user.aspx");
            }
            catch
            {
                Label7.Text = "添加失败";
            }

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("manage_user.aspx");
    }

    private void insertRecord(String tableName)     //插入记录
    {
        LoginUserInfo myUserInfo = new LoginUserInfo();
        myUserInfo = (LoginUserInfo)Session["LoginUserInfo_DT"];
        string strSql11 = "insert into lpChangeLog (usrID,usrRealName,clChangeDate,clTableName)values('" + myUserInfo.usrID + "','" + myUserInfo.realName + "','" + DateTime.Now.ToString() + "','" + tableName + "')";
        tinyDB.Update2DB(strSql11).ToString();   //写到数据库
    }
}