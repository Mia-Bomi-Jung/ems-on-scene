﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ems_onscene.Models.EntityModels;
using Microsoft.AspNet.Identity;

namespace acemsoncall.web.Controllers
{
    [Authorize]
    public class ShiftController : Controller
    {
        // GET: Shift
        public ActionResult Index()
        {
            List<AspNetUser> checkedinPersons = new List<AspNetUser>();
            using (emsonsceneEntities db = new emsonsceneEntities())
            {
                checkedinPersons = db.AspNetUsers.Where(u=>u.IsCheckedIn == true).ToList();
            }
            return View(checkedinPersons);
        }
        [HttpGet]
        public ActionResult CheckIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckIn(string MedicalRank, bool? IsCheckedIn)
        {
            using(emsonsceneEntities db = new emsonsceneEntities())
            {
                string _userId = User.Identity.GetUserId();
                var user = db.AspNetUsers.FirstOrDefault(u=>u.Id == _userId);
                if (user != null)
                {
                    var sameRankUsers = db.AspNetUsers.Where(u=>u.MedicalRank == MedicalRank && u.IsCheckedIn == true).ToList();
                    foreach(var sameUser in sameRankUsers)
                    {
                        sameUser.IsCheckedIn = false;
                    }
                    user.MedicalRank = MedicalRank;
                    user.IsCheckedIn = IsCheckedIn.HasValue?IsCheckedIn.Value:false;
                    if (IsCheckedIn.HasValue && IsCheckedIn.Value == true)
                    {
                        user.CheckedInDT = DateTime.Now;
                    }
                    db.SaveChanges();
                    ViewBag.Message = $"Successfully checked {(IsCheckedIn.HasValue&&IsCheckedIn.Value?"in":"out")}.";
                }
            }
            return View();
        }

        public ActionResult BagCheck()
        {
            return View();
        }
    }
}