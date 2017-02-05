using System;
using System.Web.Mvc;

namespace Tupperware.WebExample.Controllers
{
    public class SimpleController : Controller
    {
        private readonly IFruitStand _fruitStand;

        public SimpleController(IFruitStand fruitStand)
        {
            _fruitStand = fruitStand;
        }

        // GET: Simple
        public ActionResult Index()
        {
            ViewBag.Fruit = _fruitStand.GetRandomFruit();
            return View();
        }
    }

    public interface IFruitStand
    {
        string GetRandomFruit();
    }

    public class FruitStand: IFruitStand
    {
        private string[] _fruits;
        public FruitStand()
        {
            _fruits = new[] {"Banana", "Apple", "Cherry", "Grapefruit", "Strawberry", "Grape"};
        }

        public string GetRandomFruit()
        {
            var random = new Random();
            var index = random.Next(0, _fruits.Length);

            return _fruits[index];
        }
    }
}