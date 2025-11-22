/*
   WorkWell - Monitor de Bem-Estar
   Simulacao Wokwi com Potenciometro
*/

// Pinos
#define POT_PIN 34
#define LED_PIN 2

// Constantes
const int BASELINE_HR = 70;
const int MIN_HR = 50;
const int MAX_HR = 130;

// Variaveis
int currentHR = 75;
float currentStress = 0.0;
unsigned long lastUpdate = 0;
unsigned long lastBlink = 0;
bool ledState = false;

void setup() {
  Serial.begin(115200);
  
  pinMode(LED_PIN, OUTPUT);
  pinMode(POT_PIN, INPUT);
  
  delay(1000);
  
  Serial.println("=================================");
  Serial.println("   WorkWell - Wokwi Simulation");
  Serial.println("=================================");
  Serial.println("* Sistema inicializado");
  Serial.println("* Gire o potenciometro para");
  Serial.println("  simular batimentos cardiacos");
  Serial.println("=================================");
  Serial.println();
  
  delay(1000);
}

void loop() {
  // Ler potenciometro (0-4095)
  int potValue = analogRead(POT_PIN);
  
  // Mapear para batimentos (50-130 bpm)
  currentHR = map(potValue, 0, 4095, MIN_HR, MAX_HR);
  
  // Calcular estresse
  currentStress = calculateStress(currentHR);
  
  // Piscar LED conforme batimentos
  blinkLED(currentHR);
  
  // Exibir dados a cada 2 segundos
  if (millis() - lastUpdate >= 2000) {
    displayData();
    sendJSON();
    lastUpdate = millis();
  }
  
  delay(100);
}

float calculateStress(int heartRate) {
  float stress = (float)(heartRate - BASELINE_HR) / 30.0;
  
  if (stress < 0.0) stress = 0.0;
  if (stress > 1.0) stress = 1.0;
  
  return stress;
}

String getStressCategory(float stress) {
  if (stress < 0.3) return "Baixo";
  if (stress < 0.6) return "Moderado";
  if (stress < 0.8) return "Alto";
  return "Muito Alto";
}

String getHRCategory(int hr) {
  if (hr < 60) return "Bradicardia";
  if (hr <= 100) return "Normal";
  if (hr <= 120) return "Elevado";
  return "Taquicardia";
}

void blinkLED(int bpm) {
  int interval = 60000 / bpm;
  
  if (millis() - lastBlink >= interval) {
    ledState = !ledState;
    digitalWrite(LED_PIN, ledState);
    lastBlink = millis();
  }
}

void displayData() {
  Serial.println("-------------------------------------");
  Serial.print("  HR: ");
  Serial.print(currentHR);
  Serial.println(" bpm");
  
  Serial.print("  Stress: ");
  Serial.print(currentStress, 2);
  Serial.print(" (");
  Serial.print(getStressCategory(currentStress));
  Serial.println(")");
  
  Serial.print("  Status: ");
  Serial.println(getHRCategory(currentHR));
  Serial.println("-------------------------------------");
}

void sendJSON() {
  Serial.print("JSON: ");
  Serial.print("{\"heartRate\":");
  Serial.print(currentHR);
  Serial.print(",\"stress\":");
  Serial.print(currentStress, 2);
  Serial.println("}");
  Serial.println();
}