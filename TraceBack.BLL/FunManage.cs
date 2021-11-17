using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceBack.Model;

namespace TraceBack.BLL
{
    public class FunManage
    {
        public static string FunManageApi(WebApiPara objPara)
        {
            WebApiResult apiResult = new WebApiResult();
            switch (objPara.ApiName)
            {
                case "GetCourseList":
                    {
                        Course query;
                        if (objPara.Data == null)
                        {
                            query = new Course();
                        }
                        else
                        {
                            query = JsonConvert.DeserializeObject<Course>(objPara.Data.ToString());
                        }

                        List<Course> course = Dal.FunManage.GetCourseList(query);
                        apiResult.RecordCount = course.Count;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = course;
                    }
                    break;
                case "SaveCourse":
                    {
                        Course course;
                        if (objPara.Data == null)
                        {
                            throw new ArgumentNullException("参数不允许为空");
                        }
                        else
                        {
                            course = JsonConvert.DeserializeObject<Course>(objPara.Data.ToString());
                        }

                        using (SqlConnection connection = new SqlConnection(Dal.DbUtility.SqlHelper.ConnectionString))
                        {
                            connection.Open();
                            SqlTransaction transaction = connection.BeginTransaction();
                            try
                            {
                                Dal.FunManage.SaveCourse(course, transaction);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                            connection.Close();
                            
                        }

                        apiResult.RecordCount = 0;
                        apiResult.Result = 1;
                        apiResult.Message = "保存成功";
                        apiResult.Data = null;
                    }
                    break;
                case "GetFacilitiesList":
                    {
                        Facilities query;
                        if (objPara.Data == null)
                        {
                            query = new Facilities();
                        }
                        else
                        {
                            query = JsonConvert.DeserializeObject<Facilities>(objPara.Data.ToString());
                        }

                        List<Facilities> facilities = Dal.FunManage.GetFacilitiesList(query);
                        apiResult.RecordCount = facilities.Count;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = facilities;
                    }
                    break;
                case "SavaFacilities":
                    {
                        Facilities facilities;
                        if (objPara.Data == null)
                        {
                            throw new ArgumentNullException("参数不允许为空");
                        }
                        else
                        {
                            facilities = JsonConvert.DeserializeObject<Facilities>(objPara.Data.ToString());
                        }

                        using (SqlConnection connection = new SqlConnection(Dal.DbUtility.SqlHelper.ConnectionString))
                        {
                            connection.Open();
                            SqlTransaction transaction = connection.BeginTransaction();
                            try
                            {
                                Dal.FunManage.SaveFacilities(facilities, transaction);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                            connection.Close();
                        }

                        apiResult.RecordCount = 0;
                        apiResult.Result = 1;
                        apiResult.Message = "保存成功";
                        apiResult.Data = null;
                    }
                    break;
                case "GetNewsList":
                    {
                        News query;
                        if (objPara.Data == null)
                        {
                            query = new News();
                        }
                        else
                        {
                            query = JsonConvert.DeserializeObject<News>(objPara.Data.ToString());
                        }

                        List<News> news = Dal.FunManage.GetNewsList(query);
                        apiResult.RecordCount = news.Count;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = news;
                    }
                    break;
                case "SaveNews":
                    {
                        News news;
                        if (objPara.Data == null)
                        {
                            throw new ArgumentNullException("参数不允许为空");
                        }
                        else
                        {
                            news = JsonConvert.DeserializeObject<News>(objPara.Data.ToString());
                        }

                        using (SqlConnection connection = new SqlConnection(Dal.DbUtility.SqlHelper.ConnectionString))
                        {
                            connection.Open();
                            SqlTransaction transaction = connection.BeginTransaction();
                            try
                            {
                                Dal.FunManage.SaveNews(news, transaction);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                            connection.Close();
                        }

                        apiResult.RecordCount = 0;
                        apiResult.Result = 1;
                        apiResult.Message = "保存成功";
                        apiResult.Data = null;
                    }
                    break;
                case "GetColumnList":
                    {
                        Column query;
                        if (objPara.Data == null)
                        {
                            query = new Column();
                        }
                        else
                        {
                            query = JsonConvert.DeserializeObject<Column>(objPara.Data.ToString());
                        }

                        List<Column> column = Dal.FunManage.GetColumnList(query);
                        apiResult.RecordCount = column.Count;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = column;
                    }
                    break;
                case "SaveColumn":
                    {
                        Column column;
                        if (objPara.Data == null)
                        {
                            throw new ArgumentNullException("参数不允许为空");
                        }
                        else
                        {
                            column = JsonConvert.DeserializeObject<Column>(objPara.Data.ToString());
                        }

                        using (SqlConnection connection = new SqlConnection(Dal.DbUtility.SqlHelper.ConnectionString))
                        {
                            connection.Open();
                            SqlTransaction transaction = connection.BeginTransaction();
                            try
                            {
                                Dal.FunManage.SaveColumn(column, transaction);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                            connection.Close();
                        }

                        apiResult.RecordCount = 0;
                        apiResult.Result = 1;
                        apiResult.Message = "保存成功";
                        apiResult.Data = null;
                    }
                    break;
                default:
                    throw new ArgumentException("未知的API接口");
            }
            return JsonConvert.SerializeObject(apiResult);
        }
    }
}
