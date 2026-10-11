using DAL;
using DAL.DTOs;

namespace BAL
{
    public class EventBusiness
    {
        // ============================
        // GET ALL
        // ============================
        public static async Task<List<EventDTO>> GetAllEvents()
        {
            try
            {
                return await EventData.GetAllEvents();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all events.", ex);
            }
        }


        public static async Task<List<EventDTO>> GetAllEventsByUserId(int UserId)
        {
            try
            {
                return await EventData.GetAllEventsByUserId(UserId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all events.", ex);
            }
        }

        public static async Task<List<EventDTO>> GetAllEvents(int statusId)
        {
            try
            {
                return await EventData.GetAllEventsByStatusId(statusId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all events.", ex);
            }
        }

        public static async Task<List<StatusDTO>> GetEventStatus()
        {
            try
            {
                return await EventData.GetEventStatus();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all status.", ex);
            }
        }

        public static async Task<EventDTO?> GetEventById(int id)
        {
            try
            {
                return await EventData.GetEventById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching event with ID {id}.", ex);
            }
        }

        // ============================
        // CREATE
        // ============================
        public static async Task<(bool success, int eventId)> CreateEvent(Event_Created ev)
        {
            try
            {
                return await EventData.CreateEvent(ev);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating event.", ex);
            }
        }

        // ============================
        // UPDATE
        // ============================
        public static async Task<int> UpdateEvent(Event_Updated ev)
        {
            try
            {
                return await EventData.UpdateEvent(ev);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating event with ID {ev.EventId}.", ex);
            }
        }

        // ============================
        // DELETE
        // ============================
        public static async Task<int> DeleteEvent(int eventId)
        {
            try
            {
                return await EventData.DeleteEvent(eventId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting event with ID {eventId}.", ex);
            }
        }
    }
}