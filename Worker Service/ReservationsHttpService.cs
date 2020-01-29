using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Worker_Service
{
    public class ReservationsHttpService
    {
        private HttpClient Client;

        public ReservationsHttpService(HttpClient client, IConfiguration config)
        {
            client.BaseAddress = new Uri(config.GetValue<string>("apiUrl"));
            client.DefaultRequestHeaders.Add("Accept", "applications/json");
            client.DefaultRequestHeaders.Add("User-Agent", "reservation-processor");
            Client = client;
        }

        public async Task<bool> MarkReservationAccepted(ReservationRequest reservation)
        {
            var reservationAsString = JsonSerializer.Serialize(reservation);
            var content = new StringContent(reservationAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync("reservations/approved", content);
            return response.IsSuccessStatusCode;
        }
    }
}
