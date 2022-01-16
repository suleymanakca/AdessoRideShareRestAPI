using AdessoRideShareRestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdessoRideShareRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {

        // belirtilen parametrelere göre seyahat eklenebilir
        [HttpPost]
        public async Task<IActionResult> AddTrip([FromBody] Trip trip)
        {
            // zorunlu parametre kontrolleri
            if (string.IsNullOrEmpty(trip.Nereden))
            {
                return BadRequest("Nereden boş olamaz.");
            }

            if (string.IsNullOrEmpty(trip.Nereye))
            {
                return BadRequest("Nereye boş olamaz.");
            }

            if (trip.KoltukSayisi < 1)
            {
                return BadRequest("Koltuk Sayısı 1'den büyük olmalıdır.");
            }

            // kullanıcı kontrolü
            if (trip.UserID != 0)
            {
                var createduser = InMemoryDB.users.FirstOrDefault(x => x.UserId == trip.UserID);
                if (createduser is null)
                {
                    return BadRequest("Kullanıcı bulunamadı. Kullanıcı ID: " + trip.UserID);
                }

            }
            else
            {
                trip.UserID = InMemoryDB.AddUser();
            }

            // seyahat eklenmesi
            var id = InMemoryDB.AddTrip(trip);
            return Ok(trip);
        }




        // seyahat araması kullanıcı tarafından nereden ve nereye parametreleri kullanılarak yapılabilir.
        [HttpGet]
        public async Task<IActionResult> FindTrip([FromQuery(Name = "nereden")] string nereden, [FromQuery(Name = "nereye")] string nereye)
        {
            // parametre kontrolleri
            var Nereden_prop = !string.IsNullOrEmpty(nereden);
            var Nereye_prop = !string.IsNullOrEmpty(nereye);

            if (!Nereden_prop && !Nereye_prop)
            {
                return BadRequest("Geçersiz istek. Parametreleri giriniz.");
            }

            // parametreleri içeren seyahatler bulunur
            var sonuc = InMemoryDB.trips.Where(x => x.Yayinda &&
                        (x.Nereden.ToLower().Contains(nereden.ToLower())) &&
                        (x.Nereye.ToLower().Contains(nereye.ToLower()))).ToList();

            return Ok(sonuc);

        }


        // seyahat yayın durumu değişebilir
        [HttpPut("publish")]
        public async Task<IActionResult> UpdateTrip([FromBody] PublishRequest publishRequest)
        {
            // seyahat aranır ve gelen parametreye göre yayın durumu güncellenir
            var oldTrip = InMemoryDB.trips.FirstOrDefault(x => x.Id == publishRequest.TripId);

            if (oldTrip is not null)
            {
                oldTrip.Yayinda = publishRequest.Yayinda;

                if (publishRequest.Yayinda)
                {
                    return Ok("Seyahat yayına alındı.");
                }
                else
                {
                    return Ok("Seyahat yayından kaldırıldı.");
                }
            }
            else
            {
                return BadRequest("Seyahat bulunamadı. ID: " + publishRequest.TripId);
            }


        }

        // kullanıcı oluşan seyahatlerden birine katılabilir
        [HttpPost("join")]
        public async Task<IActionResult> JoinTrip([FromBody] UserJoin requset)
        {

            var tripID = requset.TripID;
            var userId = requset.UserID;

            // seyahat kontrolü
            var trip = InMemoryDB.trips.FirstOrDefault(x => x.Id == tripID && x.Yayinda);
            if (trip is null)
                return BadRequest("Seyahat bulunamadi. ID: " + tripID);

            // kullanıcı kontrolü
            if (userId == 0)
            {
                userId = InMemoryDB.AddUser();
                requset.UserID = userId;
            }
            else
            {
                var isUserProp = InMemoryDB.users.Any(x => x.UserId == userId);
                if (!isUserProp)
                {
                    return BadRequest("Kullanıcı kayıtlı değil. ID: " + userId);
                }
                if (trip.UserID == userId)
                {
                    return BadRequest("Oluşturduğunuz seyahate katılım isteği gönderemezsiniz.");
                }
            }

            // kayıt kontrülü
            var usersInTrip =InMemoryDB.user_join.Where(x => x.TripID == tripID).ToList();
            if (usersInTrip.Any(x => x.UserID == userId))
            {
                return Ok("Bu seyahate zaten katılım isteği göndermişsiniz.");
            }

            // boş koltuk kontrolü
            var full_seat = usersInTrip.Count;
            if (trip.KoltukSayisi == full_seat)
            {
                return BadRequest("Bu seyahatteki tum koltuklar maalesef dolu.");
            }

            InMemoryDB.AddUserJoin(requset);

            return Ok(requset);

        }


    }
}
