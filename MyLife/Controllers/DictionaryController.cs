﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MyLife.Models;
using MyLife.Context;
using System.Data.Entity;

namespace MyLife.Controllers
{
    
    public class DictionaryController : BaseController
    {
        private MyLifeContext db = new MyLifeContext();

        // GET: Dictionary
        public ActionResult DictionaryIndex()
        {
            var ParentDictionaries = from parentDictionary in db.Dictionarys
                                     where parentDictionary.ParentID == 0
                                     select parentDictionary;
            List<DictionaryUnit> DictionaryUnit = new List<Models.DictionaryUnit>();
            foreach (var dictionary in ParentDictionaries)
            {
                DictionaryUnit unit = new DictionaryUnit();
                unit.ID = dictionary.ID;
                unit.Name = dictionary.Name;

                var ChildrenDictionaries = from childrenDictionary in db.Dictionarys
                                           where childrenDictionary.ParentID == unit.ID orderby childrenDictionary.DisplayIndex
                                           select childrenDictionary;
                unit.Dictionaries = ChildrenDictionaries;
                DictionaryUnit.Add(unit);
            }
            ViewData["data"] = DictionaryUnit;
            return View();
        }

        public JsonResult AddDictionary(string name)
        {
            DictionaryModel dictionary = new DictionaryModel();
            dictionary.Name = name;
            dictionary.DisplayIndex = 0;
            dictionary.ParentID = 0;
            db.Entry(dictionary).State = EntityState.Added;
            db.SaveChanges();
            return Json(dictionary);
        }
    }
}