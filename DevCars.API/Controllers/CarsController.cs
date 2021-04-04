using DevCars.API.Entities;
using DevCars.API.InputModels;
using DevCars.API.Persistences;
using DevCars.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevCars.API.Controllers
{
    [Route("api/cars")]
    public class CarsController : ControllerBase
    {
        private readonly DevCarsDbContext _dbContext;
        public CarsController(DevCarsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET api/cars
        [HttpGet]
        public IActionResult Get()
        {
            var cars = _dbContext.Cars;

            var carsViewModel = cars
                .Select(c => new CarItemViewModel(c.Id, c.Brand, c.Model, c.Price))
                .ToList();

            return Ok(carsViewModel);
        }

        //GET api/cars/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // SE CARRO NÃO EXISTIR, RETORNA NOTFOUND (404)
            // SENÃO, OK

            var car = _dbContext.Cars.SingleOrDefault(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            var carDetailsViewModel = new CarDetailsViewModel(
                car.Id,
                car.Brand,
                car.Model,
                car.VinCode,
                car.Color,
                car.Year,
                car.Price,
                car.ProductionDate
                );

            return Ok(carDetailsViewModel);
        }

        //POST api/cars
        [HttpPost]
        public IActionResult Post([FromBody] AddCarInputModel model)
        {
            // SE O CADASTRO FUNCIONAR, RETORNA CREATED (201)
            // SE OS DADOS ESTIVEREM INCORRETOS, RETORNA BAD REQUEST (400)

            if (model.Model.Length > 50)
            {
                return BadRequest("Modelo não pode ter mais de 50 caracteres.");
            }

            var car = new Car(model.VinCode, model.Brand, model.Model, model.Year, model.Price, model.Color, model.ProductionDate);

            _dbContext.Cars.Add(car);
            _dbContext.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = car.Id },
                model
                );
        }

        //PUT api/cars/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCarInputModel model)
        {
            // SE A ATUALIZAÇÃO FUNCIONAR, RETORNA NO CONTENT (204)
            // SE OS DADOS ESTIVEREM INCORRETOS, RETORNA BAD REQUEST (400)

            if (model.Price < 0)
            {
                return BadRequest("Não é possível atualizar o preço do veículo para menor que 0.");
            }

            var car = _dbContext.Cars.SingleOrDefault(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            car.Update(model.Color, model.Price);
            _dbContext.SaveChanges();

            return NoContent();
        }

        //DELETE api/cars/2
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // SE CARRO NÃO EXISTIR, RETORNA NOTFOUND (404)
            // SE A DELEÇÃO FUNCIONAR, RETORNA NO CONTENT (204)

            var car = _dbContext.Cars.SingleOrDefault(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            car.SetAsSuspended();
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
