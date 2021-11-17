using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceBack.Model;

namespace TraceBack.Dal
{
    public class FunManage
    {
        public static List<Course> GetCourseList(Course course, SqlTransaction transaction = null)
        {
            string strSql= @"SELECT course_id,course_name,course_college,teacher_name,course_url,course_img FROM course";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (course != null)
            {
                if (course.course_id != null)
                {
                    lstCondition.Add(" course_if = @course_id ");
                    lstPara.Add(new SqlParameter("@course_id", SqlDbType.UniqueIdentifier) { Value = course.course_id });
                }

                if (!string.IsNullOrEmpty(course.course_name))
                {
                    lstCondition.Add(" course_name like @course_name ");
                    lstPara.Add(new SqlParameter("@course_name", SqlDbType.VarChar) { Value = "%" + course.course_name + "%" });
                }
            }

            // 为Sql文添加条件
            if (lstCondition.Count > 0)
            {
                strSql += " where ";
                foreach (string strCondition in lstCondition)
                {
                    strSql += strCondition + " and ";
                }
                strSql = strSql.Substring(0, strSql.Length - 4);
            }

            return DbUtility.SqlHelper.ExecuteList<Course>(
                    strSql, CommandType.Text, null, lstPara.ToArray());
        }
        public static int SaveCourse(Course course, SqlTransaction transaction = null)
        {
            if (course == null
                || string.IsNullOrEmpty(course.course_name)
                || string.IsNullOrEmpty(course.course_url)
            )
            {
                throw new ArgumentException("以下参数必需：课程名、地址");
            }
            List<SqlParameter> lstPara = new List<SqlParameter>();
            if (course.course_id == null || course.course_id == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@course_id", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@course_id", SqlDbType.UniqueIdentifier) { Value = course.course_id });
            }
            lstPara.Add(new SqlParameter("@course_name", SqlDbType.VarChar) { Value = course.course_name });
            lstPara.Add(new SqlParameter("@course_url", SqlDbType.VarChar) { Value = course.course_url });
            lstPara.Add(new SqlParameter("@course_img", SqlDbType.VarChar) { Value = course.course_img });
            
            lstPara.Add(new SqlParameter("@course_college", SqlDbType.VarChar) { Value = course.course_college });
            lstPara.Add(new SqlParameter("@teacher_name", SqlDbType.VarChar) { Value = course.teacher_name });

            lstPara.Add(new SqlParameter("@return", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
            // 返回执行结果
            DbUtility.SqlHelper.ExecuteNonQuery("SaveCourse", CommandType.StoredProcedure, transaction, lstPara.ToArray());

            object objRtn = lstPara.Where(p => p.ParameterName == "@return").FirstOrDefault().Value;

            return (int)objRtn;
        }
        public static List<News> GetNewsList(News news, SqlTransaction transaction = null)
        {
            string strSql = @"SELECT news_id,new_title,news_url,news_time,news_author,news_origin FROM news";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (news != null)
            {
                if (news.news_id != null)
                {
                    lstCondition.Add(" news_id = @news_id ");
                    lstPara.Add(new SqlParameter("@news_id", SqlDbType.UniqueIdentifier) { Value = news.news_id });
                }

                if (!string.IsNullOrEmpty(news.new_title))
                {
                    lstCondition.Add(" new_title like @new_title ");
                    lstPara.Add(new SqlParameter("@new_title", SqlDbType.VarChar) { Value = "%" + news.new_title + "%" });
                }
            }

            // 为Sql文添加条件
            if (lstCondition.Count > 0)
            {
                strSql += " where ";
                foreach (string strCondition in lstCondition)
                {
                    strSql += strCondition + " and ";
                }
                strSql = strSql.Substring(0, strSql.Length - 4);
            }

            return DbUtility.SqlHelper.ExecuteList<News>(
                    strSql, CommandType.Text, null, lstPara.ToArray());
        }
        public static int SaveNews(News news, SqlTransaction transaction = null)
        {
            if (news == null
                || string.IsNullOrEmpty(news.new_title)
                || string.IsNullOrEmpty(news.news_author)
            )
            {
                throw new ArgumentException("以下参数必需：标题、作者");
            }
            List<SqlParameter> lstPara = new List<SqlParameter>();
            if (news.news_id == null || news.news_id == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@news_id", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@news_id", SqlDbType.UniqueIdentifier) { Value = news.news_id });
            }
            lstPara.Add(new SqlParameter("@new_title", SqlDbType.VarChar) { Value = news.new_title });
            lstPara.Add(new SqlParameter("@news_url", SqlDbType.VarChar) { Value = news.news_url });
            lstPara.Add(new SqlParameter("@news_author", SqlDbType.VarChar) { Value = news.news_author });

            lstPara.Add(new SqlParameter("@news_origin", SqlDbType.VarChar) { Value = news.news_origin });

            // 返回执行结果
            DbUtility.SqlHelper.ExecuteNonQuery("SaveNews", CommandType.StoredProcedure, transaction, lstPara.ToArray());

            object objRtn = lstPara.Where(p => p.ParameterName == "@return").FirstOrDefault().Value;
            return (int)objRtn;
        }
        public static List<Facilities> GetFacilitiesList(Facilities facilities, SqlTransaction transaction = null)
        {
            string strSql = @"SELECT facilities_id,facilities_name,facilities_address,
                                facilities_people,facilities_time_start,facilities_time_end FROM facilities";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (facilities != null)
            {
                if (facilities.facilities_id != null)
                {
                    lstCondition.Add(" facilities_id = @facilities_id ");
                    lstPara.Add(new SqlParameter("@facilities_id", SqlDbType.UniqueIdentifier) { Value = facilities.facilities_id });
                }

                if (!string.IsNullOrEmpty(facilities.facilities_name))
                {
                    lstCondition.Add(" facilities_name like @facilities_name ");
                    lstPara.Add(new SqlParameter("@facilities_name", SqlDbType.VarChar) { Value = "%" + facilities.facilities_name + "%" });
                }
            }

            // 为Sql文添加条件
            if (lstCondition.Count > 0)
            {
                strSql += " where ";
                foreach (string strCondition in lstCondition)
                {
                    strSql += strCondition + " and ";
                }
                strSql = strSql.Substring(0, strSql.Length - 4);
            }

            return DbUtility.SqlHelper.ExecuteList<Facilities>(
                    strSql, CommandType.Text, null, lstPara.ToArray());
        }
        public static int SaveFacilities(Facilities facilities, SqlTransaction transaction = null)
        {
            if (facilities == null
                || string.IsNullOrEmpty(facilities.facilities_name)
                || string.IsNullOrEmpty(facilities.facilities_address)
            )
            {
                throw new ArgumentException("以下参数必需：设施名、地址");
            }
            List<SqlParameter> lstPara = new List<SqlParameter>();
            if (facilities.facilities_id == null || facilities.facilities_id == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@facilities_id", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@facilities_id", SqlDbType.UniqueIdentifier) { Value = facilities.facilities_id });
            }
            lstPara.Add(new SqlParameter("@facilities_name", SqlDbType.VarChar) { Value = facilities.facilities_name });
            lstPara.Add(new SqlParameter("@facilities_address", SqlDbType.VarChar) { Value = facilities.facilities_address });
            lstPara.Add(new SqlParameter("@facilities_people", SqlDbType.VarChar) { Value = facilities.facilities_people });

            lstPara.Add(new SqlParameter("@facilities_time_start", SqlDbType.DateTime) { Value = facilities.facilities_time_start });
            lstPara.Add(new SqlParameter("@facilities_time_end", SqlDbType.DateTime) { Value = facilities.facilities_time_end });

            // 返回执行结果
            DbUtility.SqlHelper.ExecuteNonQuery("SaveFacilities", CommandType.StoredProcedure, transaction, lstPara.ToArray());

            object objRtn = lstPara.Where(p => p.ParameterName == "@return").FirstOrDefault().Value;

            return (int)objRtn;
        }
        public static List<Column> GetColumnList(Column column, SqlTransaction transaction = null)
        {
            string strSql = @"SELECT column_id,column_name,column_parent,column_level,column_explain FROM columnnn";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (column != null)
            {
                if (column.column_id != null)
                {
                    lstCondition.Add(" column_id = @column_id ");
                    lstPara.Add(new SqlParameter("@news_id", SqlDbType.UniqueIdentifier) { Value = column.column_id });
                }

                if (!string.IsNullOrEmpty(column.column_name))
                {
                    lstCondition.Add(" column_name @column_name ");
                    lstPara.Add(new SqlParameter("@new_title", SqlDbType.VarChar) { Value = "%" + column.column_name + "%" });
                }
            }

            // 为Sql文添加条件
            if (lstCondition.Count > 0)
            {
                strSql += " where ";
                foreach (string strCondition in lstCondition)
                {
                    strSql += strCondition + " and ";
                }
                strSql = strSql.Substring(0, strSql.Length - 4);
            }

            return DbUtility.SqlHelper.ExecuteList<Column>(
                    strSql, CommandType.Text, null, lstPara.ToArray());
        }
        public static int SaveColumn(Column column, SqlTransaction transaction = null)
        {
            if (column == null
                || string.IsNullOrEmpty(column.column_name)
            )
            {
                throw new ArgumentException("以下参数必需：栏目名");
            }
            List<SqlParameter> lstPara = new List<SqlParameter>();
            if (column.column_id == null || column.column_id == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@column_id", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@column_id", SqlDbType.UniqueIdentifier) { Value = column.column_id });
            }
            lstPara.Add(new SqlParameter("@column_name", SqlDbType.VarChar) { Value = column.column_name });
            lstPara.Add(new SqlParameter("@column_level", SqlDbType.VarChar) { Value = column.column_level });
            //lstPara.Add(new SqlParameter("@column_parent", SqlDbType.UniqueIdentifier) { Value = column.column_parent });
            if (column.column_parent == null || column.column_parent == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@column_parent", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@column_parent", SqlDbType.UniqueIdentifier) { Value = column.column_parent });
            }
            lstPara.Add(new SqlParameter("@column_explain", SqlDbType.VarChar) { Value = column.column_explain });

            // 返回执行结果
            DbUtility.SqlHelper.ExecuteNonQuery("SaveColumn", CommandType.StoredProcedure, transaction, lstPara.ToArray());
            Console.WriteLine(lstPara.ToArray());

            object objRtn = lstPara.Where(p => p.ParameterName == "@return").FirstOrDefault().Value;

            return (int)objRtn;
        }
    }
}
