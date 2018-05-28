﻿using ChocOvation.Models;
using ChocOvation.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChocOvation.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order
        public async Task<ActionResult> Index()
        {
            var orders = db.Orders.Include(o => o.ChosenOffer).Include(o => o.ChosenOffer.Supplier);
            return View(await orders.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create(int id)
        {
            //ViewBag.OfferID = new SelectList(db.Offers, "OfferID", "SupplierID");

            var order = new Order()
            {
                OfferID = id,
                DateOfOrder = DateTime.Today.Date
            };
            db.Orders.Add(order);
            db.SaveChanges();

            var LittleOrders = new List<OrderFormViewModel>();
            var offerperMat = db.OffersPerMaterials.Include(m => m.Material).Where(o => o.OfferID == id).ToList();

            for (var mat = 0; mat < offerperMat.Count(); mat++)
            {
                var materialOffer = offerperMat[mat];
                
                var matName = db.Materials
                    .Where(m => m.MaterialName == materialOffer.Material.MaterialName)
                    .Select(m => m.MaterialName)
                    .FirstOrDefault();

                var littleOrder = new OrderFormViewModel();
                littleOrder.MaterialName = matName;
                //littleOrder.OrderID = order.OrderID;

                if (littleOrder.MaterialName == "Cocoa")
                {
                    littleOrder.QuantityPerYear = 20500 * 350;
                }
                else if (littleOrder.MaterialName == "Sugar")
                {
                    littleOrder.QuantityPerYear = 12500 * 350;

                }
                else if (littleOrder.MaterialName == "Milk")
                {
                    littleOrder.QuantityPerYear = 7500 * 350;

                }
                else if (littleOrder.MaterialName == "Butter")
                {
                    littleOrder.QuantityPerYear = 7500 * 350;

                }
                else if (littleOrder.MaterialName == "Almonds")
                {
                    littleOrder.QuantityPerYear = 1000 * 365;

                }
                else if (littleOrder.MaterialName == "Hazelnuts")
                {
                    littleOrder.QuantityPerYear = 1000 * 365;

                }

                LittleOrders.Add(littleOrder);
            }
            var listOfLittleOrders = LittleOrders.AsEnumerable();

            return View(listOfLittleOrders);
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "QuantityPerYear, MaterialName")] IEnumerable<OrderFormViewModel> viewModel)
        {


            if (ModelState.IsValid)
            {

                foreach (var item in viewModel)
                {
                    var littleOffer = new OrderPerMaterial
                    {
                        //OrderID = item.OrderID,
                        Material = db.Materials.Where(m => m.MaterialName == item.MaterialName).Single(),
                        QuantityPerYear = item.QuantityPerYear
                    };

                    db.OrderPerMaterials.Add(littleOffer);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index");

            }
            return View(viewModel);

        }

        // GET: Order/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfferID = new SelectList(db.Offers, "OfferID", "SupplierID", order.OfferID);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrderID,Quantity,DateOfOrder,OfferID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OfferID = new SelectList(db.Offers, "OfferID", "SupplierID", order.OfferID);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
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
    }
}
