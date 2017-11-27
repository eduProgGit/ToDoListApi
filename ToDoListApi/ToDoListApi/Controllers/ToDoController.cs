using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApi.Model;

namespace ToDoListApi
{
    [Route("api/[controller]")]
    public class ToDoController : Controller{
        private readonly ToDoContext _context;
        private readonly ToDoItem item;

        public ToDoController(ToDoContext context){
            _context = context;

            if (_context.Items.Count() == 0)
            {
                _context.Items.Add(new ToDoItem
                {
                    Name = "Hacer la compra"
                });

                _context.Items.Add(new ToDoItem
                {
                    Name = "Sacar el perro"
                });

                _context.SaveChanges();
            }
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ToDoItem> Get(){
            return _context.Items;
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetToDoItem")]
        public async Task<IActionResult> Get([FromRoute] int id){
            // COmprueba si los parametros que recibe son correctos api/1 o api/abc
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Proceso asincrono para obtener result de la BBDD
            var item = await _context.Items.SingleOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ToDoItem item){
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetToDoItem", new { id = item.Id }, item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]ToDoItem item){
            if (!ModelState.IsValid || id != item.Id){
                return BadRequest(ModelState);
            }

            _context.Entry(item).State = EntityState.Modified;

            try{
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Items.Any(x => x.Id == id)){
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Items.SingleOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
