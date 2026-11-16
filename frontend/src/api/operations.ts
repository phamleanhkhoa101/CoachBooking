export interface TripSchedule {
  scheduleId: number; name: string; vehicleId: number; routeId: number; driverId: number;
  departureTime: string; weekdays: string; validFrom: string; validTo: string;
  isActive: boolean; excludedDates: string; lastResult: string | null; lastRunAt: string | null;
}
export interface GenerationResult { serviceDate: string; status: string; tripId: number | null; message: string }
