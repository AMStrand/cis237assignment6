using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Models;

namespace cis237assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageAMahlerEntities db = new BeverageAMahlerEntities();

        // GET: Beverages
        public ActionResult Index()
        {
                // Set up a local variable to hold the beverages data set:
            DbSet<Beverage> BeveragesToSearch = db.Beverages;

                // Set up strings to hold data from the filter, set to null:
            string filterName = "";
            string filterPack = "";
            string filterMinPrice = "";
            string filterMaxPrice = "";
                // Set up decimal variables for the price constraints:
            decimal minPrice = 0;
            decimal maxPrice = 1000;

                // Check to see if there is a value specified for each filter type, 
                // and if so, set the local variable to that value:
            if (!String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];
            }
            if (!String.IsNullOrWhiteSpace((string)Session["pack"]))
            {
                filterPack = (string)Session["pack"];
            }
            if (!String.IsNullOrWhiteSpace((string)Session["minPrice"]))
            {
                filterMinPrice = (string)Session["minPrice"];
                minPrice = decimal.Parse(filterMinPrice);
            }
            if (!String.IsNullOrWhiteSpace((string)Session["maxPrice"]))
            {
                filterMaxPrice = (string)Session["maxPrice"];
                maxPrice = decimal.Parse(filterMaxPrice);
            }

                // Set up a filtered data set based on the filter values:
            IEnumerable<Beverage> filtered = BeveragesToSearch.Where(bev => bev.name.Contains(filterName) &&
                bev.pack.Contains(filterPack) && bev.price >= minPrice && bev.price <= maxPrice);

                // Place the values into the ViewBag for retrieval:
            ViewBag.filterName = filterName;
            ViewBag.filterPack = filterPack;
            ViewBag.filterMinPrice = filterMinPrice;
            ViewBag.filterMaxPrice = filterMaxPrice;

                // Return the filtered view:
            return View(filtered);
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

            // Method to store the filter data in the session and refresh the Index page:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
                // Get the form data sent out of the Request object;
                // The string used as a key matches the name of the form control.
            string name = Request.Form.Get("name");
            string pack = Request.Form.Get("pack");
            string minPrice = Request.Form.Get("minPrice");
            string maxPrice = Request.Form.Get("maxPrice");

                // Store the form data in the session to be retrieved later:
            Session["name"] = name;
            Session["pack"] = pack;
            Session["minPrice"] = minPrice;
            Session["maxPrice"] = maxPrice;
               
                // Redirect the user to the Index page:
            return RedirectToAction("Index");
        }
    }
}
