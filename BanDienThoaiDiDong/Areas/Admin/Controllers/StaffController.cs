using BanDienThoaiDiDong.Models;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

namespace BanDienThoaiDiDong.Areas.Admin.Controllers
{
    public class StaffController : Controller
    {
        private DB_DiDongEntities db = new DB_DiDongEntities();
        // GET: Admin/Staff

        [HttpGet]
        public ActionResult DSNV()
        {
            ViewBag.FileError = TempData["FileError"];
            ViewBag.ImportSuccess = TempData["ImportSuccess"];
            ViewBag.ImportError = TempData["ImportError"];
            var nhanvien = db.NHANVIENs.ToList();
            return View(nhanvien);
        }


        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            if(file != null && file.ContentLength > 0) 
            {
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                if (fileExtension != ".xlsx")
                {
                    TempData["FileError"] = "Invalid file type. Please upload an Excel file (.xlsx).";
                    return RedirectToAction("DSNV");
                }
            }

            try
            {
                if(file != null && file.ContentLength > 0)
                {
                    using (var stream = file.InputStream)
                    {
                        //Open the Excel file using OpenXML SDK
                        using (var document = SpreadsheetDocument.Open(stream, false))
                        {
                            var workbookPart = document.WorkbookPart;
                            var sheet = workbookPart.WorksheetParts.First().Worksheet;

                            var sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;

                            // delete all data to avoid duplicated data
                            db.NHANVIENs.RemoveRange(db.NHANVIENs);
                            //Get the data from second row
                            for (int i = 1; i < sheet.Descendants<Row>().Count(); i++)
                            {
                                var row = sheet.Descendants<Row>().ElementAt(i);
                                string name = null;
                                var nameCell = row.Descendants<Cell>().ElementAtOrDefault(0);
                                if (nameCell!= null && !string.IsNullOrEmpty(nameCell.InnerText))
                                {
                                    name = GetValueFromCell(nameCell, sharedStringTable);
                                }
                                string email = null;
                                var emailCell = row.Descendants<Cell>().ElementAtOrDefault(1);
                                if (emailCell != null && !string.IsNullOrEmpty(emailCell.InnerText))
                                {
                                    email = GetValueFromCell(emailCell, sharedStringTable);
                                }
                                string address = null;
                                var addressCell = row.Descendants<Cell>().ElementAtOrDefault(2);
                                if (addressCell != null && !string.IsNullOrEmpty(addressCell.InnerText))
                                {
                                    address = GetValueFromCell(addressCell, sharedStringTable);
                                }
                                string phone = null;
                                var phoneCell = row.Descendants<Cell>().ElementAtOrDefault(3);
                                if (phoneCell != null && !string.IsNullOrEmpty(phoneCell.InnerText))
                                { 
                                    phone = GetValueFromCell(phoneCell, sharedStringTable);
                                }
                                string username = null;
                                var usernameCell = row.Descendants<Cell>().ElementAtOrDefault(4);
                                if (usernameCell != null && !string.IsNullOrEmpty(usernameCell.InnerText))
                                {
                                    username = GetValueFromCell(usernameCell, sharedStringTable);
                                }
                                string password = null;
                                var passwordCell = row.Descendants<Cell>().ElementAtOrDefault(5);
                                if (passwordCell != null && !string.IsNullOrEmpty(passwordCell.InnerText))
                                {
                                    password = GetValueFromCell(passwordCell, sharedStringTable);
                                }

                                //
                                /*var name = GetValueFromCell(row.Descendants<Cell>().ElementAt(0), sharedStringTable);
                                var emailCell = row.Descendants<Cell>().Count() > 1 ? row.Descendants<Cell>().ElementAt(1) : null;
                                var email = emailCell != null ? GetValueFromCell(emailCell, sharedStringTable) : null;
                                var addressCell = row.Descendants<Cell>().Count() > 1 ? row.Descendants<Cell>().ElementAt(2) : null;
                                var address = addressCell != null ? GetValueFromCell(addressCell, sharedStringTable) : null;
                                var phoneCell = row.Descendants<Cell>().Count() > 1 ? row.Descendants<Cell>().ElementAt(3) : null;
                                var phone = phoneCell != null ? GetValueFromCell(phoneCell, sharedStringTable) : null;
                                var usernameCell = row.Descendants<Cell>().Count() > 1 ? row.Descendants<Cell>().ElementAt(4) : null;
                                var username = usernameCell != null ? GetValueFromCell(usernameCell, sharedStringTable) : null;
                                var passwordCell = row.Descendants<Cell>().Count() > 1 ? row.Descendants<Cell>().ElementAt(1) : null;
                                var password = passwordCell != null ? GetValueFromCell(passwordCell, sharedStringTable) : null;*/
                                int? role = null;
                                var roleCell = row.Descendants<Cell>().ElementAtOrDefault(6);
                                if (roleCell != null && !string.IsNullOrEmpty(roleCell.InnerText))
                                {
                                    role = int.Parse(GetValueFromCell(roleCell, sharedStringTable));
                                }

                                //stop the loop when value is blank 
                                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                                    break;
                                //add data to database
                                var staff = new NHANVIEN
                                {
                                    HoTen = name,
                                    Email = email,
                                    DiaChi = address,
                                    SoDienThoai = phone,
                                    UserName = username,
                                    MatKhau = password,
                                    Roles = role,
                                };
                                db.NHANVIENs.Add(staff);
                            }
                            //Save changes
                            db.SaveChanges();
                            TempData["ImportSuccess"] = "Imported successfully";
                            return RedirectToAction("DSNV"); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ImportError"] = $"Error importing file: {ex.Message}";
            }
            return RedirectToAction("DSNV");
        }


        private string GetValueFromCell(Cell cell, SharedStringTable sharedStringTable)
        {
            if (cell.DataType == null)
            {
                return cell.InnerText;
            }
            else if (cell.DataType == CellValues.SharedString)
            {
                var index = int.Parse(cell.InnerText);
                return sharedStringTable.ElementAt(index).InnerText;
            }
            else
            {
                return cell.InnerText;
            }
        }
    }
}