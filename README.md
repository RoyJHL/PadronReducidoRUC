# RUC Search API

Servicio **Web API en C#** que proporciona funcionalidad para buscar uno o múltiples valores dentro del archivo de texto delimitado "**Padrón reducido RUC**", utilizando lectura por bloques y procesamiento paralelo para maximizar el rendimiento.

## 🚀 Características

- Búsqueda ultra rápida en archivos de texto muy grandes (1GB o más)
- Soporte para múltiples valores a buscar
- Procesamiento paralelo por bloques
- Lectura optimizada sin cargar el archivo completo en memoria
- API REST simple para integrarse con otros sistemas

## 🛠 Tecnologías utilizadas

- C#
- ASP.NET Web API (.NET Framework 4.8)
- Paralelización
- Lectura de archivos eficiente con `FileStream` y buffers
- Comparación byte a byte para mayor rendimiento

## ⚙️ Requisitos

- .NET Framework 4.8
- Visual Studio 2019/2022 o superior
- Archivo del padrón reducido RUC en formato `.txt` delimitado (por ejemplo, con `|`)

## 📦 Instalación y ejecución

1. Clona este repositorio:
2. Abre la solución en Visual Studio.
3. Asegúrate de tener el archivo de entrada (padrón RUC) en la ubicación configurada, se puede descargar desde https://www.sunat.gob.pe/descargaPRR/mrc137_padron_reducido.html
4. Compila y ejecuta el proyecto.
5. Prueba la API usando Postman, Insomnia, Bruno o desde tu frontend.

## 📥 Ejemplo de uso

**Endpoint**
```bash
POST /api/ruc
```

**Request body (JSON)**
```json
{
    "rucs": [
        "10427040319",
        "20407324464",
        "20602324371"
    ]
}
```

**Response (JSON)**
```json
{
    "success": true,
    "data": {
        "10427040319": {
            "ruc": "10427040319",
            "nombre_o_razon_social": "GUERRERO ROSALES JUSTINO PEDRO",
            "direccion": "",
            "estado": "ACTIVO",
            "condicion": "HABIDO",
            "ubigeo_sunat": ""
        },
        "20407324464": {
            "ruc": "20407324464",
            "nombre_o_razon_social": "ASOC.CIVIL FONGAL LOS LIBERTADORES WARI",
            "direccion": "AV. INDEPENDENCIA NRO. S/N",
            "estado": "BAJA DE OFICIO",
            "condicion": "NO HABIDO",
            "ubigeo_sunat": "050101"
        },
        "20602324371": {
            "ruc": "20602324371",
            "nombre_o_razon_social": "ALBANICO E.I.R.L.",
            "direccion": "PRO. LA FRAGATA NRO. 3 AGRU MARBELLA DPTO. 103",
            "estado": "BAJA DE OFICIO",
            "condicion": "HABIDO",
            "ubigeo_sunat": "150120"
        }
    },
    "message": {
        "type": 1,
        "text": "Se realizó la búsqueda exitosamente."
    },
    "datetime": "2025-03-22T07:45:14.9188281Z"
}
```

## 🎥 Vídeo de funcionamiento
[![Ver en YouTube](https://img.youtube.com/vi/u0HGeOgBcGs/0.jpg)](https://www.youtube.com/watch?v=u0HGeOgBcGs)