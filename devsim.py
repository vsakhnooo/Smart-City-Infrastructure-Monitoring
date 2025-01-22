import random
import time
import json
from azure.iot.device import IoTHubDeviceClient
import math

# Initialize IoT Hub client
def initialize_client(connection_string):
    return IoTHubDeviceClient.create_from_connection_string(connection_string)

# Traffic Sensor Data Generation
def generate_traffic_data(sensor_id, road_names):
    hour = time.localtime().tm_hour
    base_vehicle_count = int(30 + 170 * math.sin(math.pi * (hour - 6) / 12))
    road_name = random.choice(road_names)

    vehicle_count = base_vehicle_count + random.randint(-10, 10)
    if vehicle_count < 0:
        vehicle_count = random.randint(10, 30)

    return {
        "sensor_id": sensor_id,
        "road_name": road_name,
        "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
        "vehicle_count": vehicle_count,
        "average_speed": max(10, round(60 - base_vehicle_count / 5 + random.uniform(-5, 5), 2)),
    }

# Air Quality Sensor Data Generation
def generate_air_quality_data(sensor_id, locations):
    hour = time.localtime().tm_hour
    pm2_5_base = 20 if 6 <= hour <= 22 else 10
    pm10_base = 40 if 6 <= hour <= 22 else 20
    co2_base = 400 if 6 <= hour <= 22 else 350

    location = random.choice(locations)

    return {
        "sensor_id": sensor_id,
        "location": location,
        "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
        "pm2_5": round(pm2_5_base + random.uniform(-5, 5), 2),
        "pm10": round(pm10_base + random.uniform(-10, 10), 2),
        "co2": random.randint(co2_base - 20, co2_base + 20),
    }

# Energy Consumption Sensor Data Generation
def generate_energy_data(sensor_id, building_names):
    hour = time.localtime().tm_hour
    base_energy_consumed = 100 if 8 <= hour <= 18 else 50
    base_power_usage = 20 if 8 <= hour <= 18 else 10

    building_name = random.choice(building_names)

    return {
        "sensor_id": sensor_id,
        "building_name": building_name,
        "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
        "energy_consumed": round(base_energy_consumed + random.uniform(-20, 20), 2),
        "power_usage": round(base_power_usage + random.uniform(-5, 5), 2),
    }

# Main function to run all sensors sequentially
def run_sensors():
    # IoT Hub connection string (replace with your actual connection string)
    CONNECTION_STRING = "HostName=smartcity-iothub.azure-devices.net;DeviceId=Sensor_Simulator01;SharedAccessKey=EhsJWpiNTa15fGumumir4rUgMJChgfZ0UATAWH/QGm0="
    client = initialize_client(CONNECTION_STRING)

    # Define multiple locations for each sensor type
    traffic_roads = ["Main Street", "Highway 1", "City Center", "Downtown"]
    air_quality_locations = ["City Park", "Industrial Area", "Residential Zone"]
    energy_buildings = ["Building A", "Building B", "Office Tower", "Mall"]

    # Set send frequencies (in seconds) for each sensor type
    send_frequencies = {
        "traffic": 10,
        "air_quality": 15,
        "energy": 20,
    }

    while True:
        try:
            # Generate and send traffic data
            traffic_data = generate_traffic_data("TS001", traffic_roads)
            client.send_message(json.dumps(traffic_data))
            print(f"Traffic data sent: {traffic_data}")
            time.sleep(send_frequencies["traffic"])

            # Generate and send air quality data
            air_quality_data = generate_air_quality_data("AQ001", air_quality_locations)
            client.send_message(json.dumps(air_quality_data))
            print(f"Air quality data sent: {air_quality_data}")
            time.sleep(send_frequencies["air_quality"])

            # Generate and send energy data
            energy_data = generate_energy_data("EN001", energy_buildings)
            client.send_message(json.dumps(energy_data))
            print(f"Energy data sent: {energy_data}")
            time.sleep(send_frequencies["energy"])

        except Exception as e:
            print(f"Error in sensor simulation: {e}")
            time.sleep(5)  # Pause before retrying

if __name__ == "__main__":
    run_sensors()
