const trafficVehicleCount = document.getElementById('trafficVehicleCount').getContext('2d');
const trafficAvgSpeed = document.getElementById('trafficAvgSpeed').getContext('2d');
const airQualityPM25 = document.getElementById('airQualityPM25').getContext('2d');
const airQualityPM10 = document.getElementById('airQualityPM10').getContext('2d');
const airQualityCO2 = document.getElementById('airQualityCO2').getContext('2d');
const energyConsumed = document.getElementById('energyConsumed').getContext('2d');
const energyPowerUsage = document.getElementById('energyPowerUsage').getContext('2d');

let trafficData = [];
let airQualityData = [];
let energyData = [];

// Load data function
async function loadData() {
    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;

    await Promise.all([
        loadTrafficData(startDate, endDate),
        loadAirQualityData(startDate, endDate),
        loadEnergyData(startDate, endDate),
    ]);
}

// Fetch Traffic Data
async function loadTrafficData(startDate, endDate) {
    const url = `https://smartcity-api-ezasg2gzg8g7czhs.northeurope-01.azurewebsites.net/api/Traffic?startDate=${startDate}&endDate=${endDate}`;
    const response = await fetch(url);
    const data = await response.json();
    trafficData = data.map(d => ({
        timestamp: d.timestamp,
        vehicleCount: d.vehicle_count,
        avgSpeed: d.average_speed
    }));
    updateTrafficCharts();
}

// Fetch Air Quality Data
async function loadAirQualityData(startDate, endDate) {
    const url = `https://smartcity-api-ezasg2gzg8g7czhs.northeurope-01.azurewebsites.net/api/AirQuality?startDate=${startDate}&endDate=${endDate}`;
    const response = await fetch(url);
    const data = await response.json();
    airQualityData = data.map(d => ({
        timestamp: d.timestamp,
        pm2_5: d.pm2_5,
        pm10: d.pm10,
        co2: d.co2
    }));
    updateAirQualityCharts();
}

// Fetch Energy Data
async function loadEnergyData(startDate, endDate) {
    const url = `https://smartcity-api-ezasg2gzg8g7czhs.northeurope-01.azurewebsites.net/api/Energy?startDate=${startDate}&endDate=${endDate}`;
    const response = await fetch(url);
    const data = await response.json();
    energyData = data.map(d => ({
        timestamp: d.timestamp,
        energyConsumed: d.energy_consumed,
        powerUsage: d.power_usage
    }));
    updateEnergyCharts();
}

// Update Traffic Charts
function updateTrafficCharts() {
    const labels = trafficData.map(d => d.timestamp);
    const vehicleCount = trafficData.map(d => d.vehicleCount);
    const avgSpeed = trafficData.map(d => d.avgSpeed);

    // Vehicle Count Chart
    new Chart(trafficVehicleCount, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Vehicle Count',
                data: vehicleCount,
                borderColor: 'rgba(255, 99, 132, 1)',
                fill: false
            }]
        }
    });

    // Average Speed Chart
    new Chart(trafficAvgSpeed, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Average Speed',
                data: avgSpeed,
                borderColor: 'rgba(75, 192, 192, 1)',
                fill: false
            }]
        }
    });
}

// Update Air Quality Charts
function updateAirQualityCharts() {
    const labels = airQualityData.map(d => d.timestamp);
    const pm25 = airQualityData.map(d => d.pm2_5);
    const pm10 = airQualityData.map(d => d.pm10);
    const co2 = airQualityData.map(d => d.co2);

    // PM2.5 Chart
    new Chart(airQualityPM25, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'PM2.5',
                data: pm25,
                borderColor: 'rgba(255, 159, 64, 1)',
                fill: false
            }]
        }
    });

    // PM10 Chart
    new Chart(airQualityPM10, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'PM10',
                data: pm10,
                borderColor: 'rgba(153, 102, 255, 1)',
                fill: false
            }]
        }
    });

    // CO2 Chart
    new Chart(airQualityCO2, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'CO2',
                data: co2,
                borderColor: 'rgba(255, 159, 64, 1)',
                fill: false
            }]
        }
    });
}

// Update Energy Charts
function updateEnergyCharts() {
    const labels = energyData.map(d => d.timestamp);
    const consumedEnergyData = energyData.map(d => d.energyConsumed);
    const powerUsage = energyData.map(d => d.powerUsage);

    // Energy Consumed Chart
    new Chart(energyConsumed, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Energy Consumed',
                data: consumedEnergyData,
                borderColor: 'rgba(255, 99, 132, 1)',
                fill: false
            }]
        }
    });

    // Power Usage Chart
    new Chart(energyPowerUsage, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Power Usage',
                data: powerUsage,
                borderColor: 'rgba(75, 192, 192, 1)',
                fill: false
            }]
        }
    });
}

// Initialize the page by loading data
loadData();
