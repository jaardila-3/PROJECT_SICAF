using SICAF.Data.Entities.Academic;

using static SICAF.Common.Constants.AviationConstants;

namespace SICAF.Data.Initialization;

public static class MissionTaskRotaryWingData
{
    public static List<Tasks> GetRotaryWingTasks()
    {
        string rotaryWing = WingTypes.ROTARY_WING;
        return
        [
            new() { Code = 1111, Name = "CONOCIMIENTOS PREVIOS AL VUELO", WingType = rotaryWing },
            new() { Code = 3333, Name = "CONTROL DIRECCIONAL", WingType = rotaryWing },
            new() { Code = 4444, Name = "CONTROL COLECTIVO", WingType = rotaryWing },
            new() { Code = 5555, Name = "CONTROL CICLICO", WingType = rotaryWing },
            new() { Code = 6666, Name = "PLANIFICACIÓN DE VUELO TÁCTICO", WingType = rotaryWing },
            new() { Code = 7777, Name = "EJECUTAR PLANTEAMIENTO DE MISIÓN TÁCTICA", WingType = rotaryWing },
            new() { Code = 8888, Name = "MANIOBRAS DE VUELO IFR", WingType = rotaryWing },
            new() { Code = 9000, Name = "CONTROL DE CABECEO", WingType = rotaryWing },
            new() { Code = 9001, Name = "CONTROL DE BANQUEO", WingType = rotaryWing },
            new() { Code = 9002, Name = "CONTROL DE POTENCIA", WingType = rotaryWing },
            new() { Code = 9003, Name = "CONTROL DE ALTITUD", WingType = rotaryWing },
            new() { Code = 9004, Name = "CONTROL DE RUMBO", WingType = rotaryWing },
            new() { Code = 9005, Name = "VUELO RECTO Y NIVELADO", WingType = rotaryWing },
            new() { Code = 9006, Name = "'SS' VERTICALES", WingType = rotaryWing },
            new() { Code = 9007, Name = "VIRAJES A RUMBO PREDETERMINADO", WingType = rotaryWing },
            new() { Code = 9008, Name = "PATRON 'A Y B'", WingType = rotaryWing },
            new() { Code = 9009, Name = "ORIENTACION CON CI", WingType = rotaryWing },
            new() { Code = 9010, Name = "ORIENTACION CON HSI", WingType = rotaryWing },
            new() { Code = 9011, Name = "ORIENTACION CON RMI", WingType = rotaryWing },
            new() { Code = 9012, Name = "HOMING CON CI", WingType = rotaryWing },
            new() { Code = 9013, Name = "HOMING CON HSI", WingType = rotaryWing },
            new() { Code = 9014, Name = "HOMING CON RMI", WingType = rotaryWing },
            new() { Code = 9015, Name = "TRACKING CON CI", WingType = rotaryWing },
            new() { Code = 9016, Name = "TRACKING CON HSI", WingType = rotaryWing },
            new() { Code = 9017, Name = "TRACKING CON RMI", WingType = rotaryWing },
            new() { Code = 9018, Name = "INTERCEPTAR RADIALES CON CI", WingType = rotaryWing },
            new() { Code = 9019, Name = "INTERCETAR RADIALES CON HSI", WingType = rotaryWing },
            new() { Code = 9020, Name = "INTERCEPTAR QDR CON RMI", WingType = rotaryWing },
            new() { Code = 9021, Name = "CALCULOS DE TIEMPO Y DISTANCIA", WingType = rotaryWing },
            new() { Code = 9022, Name = "INCORPORACION AL CIRCUITO", WingType = rotaryWing },
            new() { Code = 9023, Name = "USO DE NEMOTECNIAS", WingType = rotaryWing },
            new() { Code = 9024, Name = "COMPUTADOR DE VUELO", WingType = rotaryWing },
            new() { Code = 9025, Name = "APROXIMACION CON VOR - DME", WingType = rotaryWing },
            new() { Code = 9026, Name = "APROXIMACION CON ILS", WingType = rotaryWing },
            new() { Code = 9027, Name = "SALIDAS NORMALIZADAS 'SIDS'", WingType = rotaryWing },
            new() { Code = 9028, Name = "LLEGADAS NORMALIZADAS 'STARS'", WingType = rotaryWing },
            new() { Code = 9029, Name = "CRUCEROS IFR", WingType = rotaryWing },
            new() { Code = 9030, Name = "PUNTOS DE NOTIFICACION", WingType = rotaryWing },
            new() { Code = 9031, Name = "ARCOS DME", WingType = rotaryWing },
            new() { Code = 9032, Name = "FIJO A FIJO", WingType = rotaryWing },
            new() { Code = 9033, Name = "CIRCUITOS NO PUBLICADOS", WingType = rotaryWing },

            new() { Code = 1000, Name = "PARTICIPAR EN LA ORIENTACIÓN DE LA TRIPULACIÓN", WingType = rotaryWing },
            new() { Code = 1002, Name = "CONDUCIR ORIENTACIÓN A LOS PASAJEROS", WingType = rotaryWing },
            new() { Code = 1004, Name = "PLANIFICAR UN VUELO VFR", WingType = rotaryWing },
            new() { Code = 1006, Name = "PLANIFICAR UN VUELO IFR", WingType = rotaryWing },
            new() { Code = 1010, Name = "PREPARAR CARTA DE PLANIFICACIÓN Y RENDIMIENTO", WingType = rotaryWing },
            new() { Code = 1012, Name = "VERIFICAR EL PESO Y BALANCE DE LA AERONAVE", WingType = rotaryWing },
            new() { Code = 1014, Name = "OPERAR EQUIPO DE SUPERVIVENCIA DE AVIACION", WingType = rotaryWing },
            new() { Code = 1022, Name = "EJECUTAR INSPECCION PRE VUELO", WingType = rotaryWing },
            new() { Code = 1024, Name = "EJECUTAR CHEQUEOS ANTES DEL ARRANQUE DEL MOTOR HASTA ANTES DE RETIRARSE DE LA AERONAVE", WingType = rotaryWing },
            new() { Code = 1026, Name = "MANTENER VIGILANCIA DEL ESPACIO AEREO", WingType = rotaryWing },
            new() { Code = 1028, Name = "EJECUTAR CHEQUEO DE POTENCIA EN VUELO ESTACIONARIO", WingType = rotaryWing },
            new() { Code = 1030, Name = "EJECUTAR CHEQUEO DE ESTACIONARIO OGE", WingType = rotaryWing },
            new() { Code = 1031, Name = "EJECUTAR DESPEGUE DE MAXIMO RENDIMIENTO SIMULADO", WingType = rotaryWing },
            new() { Code = 1032, Name = "EJECUTAR PROCEDIMIENTOS DE RADIO COMUNICACIÓN", WingType = rotaryWing },
            new() { Code = 1038, Name = "EJECUTAR VUELO ESTACIONARIO", WingType = rotaryWing },
            new() { Code = 1040, Name = "EJECUTAR DESPEGUE VMC", WingType = rotaryWing },
            new() { Code = 1041, Name = "EJECUTAR PATRÓN DE TRÁFICO", WingType = rotaryWing },
            new() { Code = 1044, Name = "NAVEGAR POR PILOTAJE Y ESTIMA", WingType = rotaryWing },
            new() { Code = 1048, Name = "EJECUTAR PROCEDIMIENTOS ADMINISTRACIÓN DE COMBUSTIBLE", WingType = rotaryWing },
            new() { Code = 1052, Name = "EJECUTAR MANIOBRAS DE VUELO VMC", WingType = rotaryWing },
            new() { Code = 1058, Name = "EJECUTAR APROXIMACIÓN VMC", WingType = rotaryWing },
            new() { Code = 1062, Name = "EJECUTAR OPERACIONES EN DECLIVES", WingType = rotaryWing },
            new() { Code = 1063, Name = "EJECUTAR OPERACIONES DE ÁREA CONFINADA", WingType = rotaryWing },
            new() { Code = 1066, Name = "EJECUTAR ATERRIZAJE CORRIDO", WingType = rotaryWing },
            new() { Code = 1068, Name = "EJECUTAR IDA AL AIRE", WingType = rotaryWing },
            new() { Code = 1070, Name = "RESPONDER A EMERGENCIAS", WingType = rotaryWing },
            new() { Code = 1072, Name = "EJECUTAR FALLA SIMULADA DEL MOTOR EN ESTACIONARIO", WingType = rotaryWing },
            new() { Code = 1074, Name = "EJECUTAR FALLA SIMULADA DEL MOTOR EN VUELO CRUCERO", WingType = rotaryWing },
            new() { Code = 1075, Name = "DOMINIO DEL SISTEMA HIDRÁULICO EN VUELO ESTACIONARIO", WingType = rotaryWing },
            new() { Code = 1076, Name = "EJECUTAR FALLA SIMULADA DEL SISTEMA HIDRAULICO", WingType = rotaryWing },
            new() { Code = 1080, Name = "EJECUTAR O DESCRIBIR PROCEDIMIENTOS PARA FALLA DE COMUNICACIÓN POR RADIOS", WingType = rotaryWing },
            new() { Code = 1082, Name = "EJECUTAR AUTORROTACIÓN", WingType = rotaryWing },
            new() { Code = 1155, Name = "LIBRAR OBSTÁCULOS DE CABLES", WingType = rotaryWing },
            new() { Code = 1162, Name = "EJECUTAR SALIDA DE EMERGENCIA", WingType = rotaryWing },
            new() { Code = 1170, Name = "EJECUTAR DESPEGUE POR INSTRUMENTOS", WingType = rotaryWing },
            new() { Code = 1172, Name = "EJECUTAR RADIONAVEGACIÓN", WingType = rotaryWing },
            new() { Code = 1174, Name = "EJECUTAR PROCEDIMIENTOS DE PATRÓN DE ESPERA", WingType = rotaryWing },
            new() { Code = 1176, Name = "EJECUTAR APROXIMACION NO PRECISA", WingType = rotaryWing },
            new() { Code = 1178, Name = "EJECUTAR APROXIMACION PRECISA", WingType = rotaryWing },
            new() { Code = 1182, Name = "EJECUTAR RECOBRO DE ACTITUD NO USUAL", WingType = rotaryWing },
            new() { Code = 1184, Name = "RESPONDER A IMC INADVERTIDA", WingType = rotaryWing },
            new() { Code = 1190, Name = "EJECUTAR SEÑALES DE MANO Y BRAZO", WingType = rotaryWing },
            new() { Code = 1194, Name = "EJECUTAR OPERACIONES DE REABASTECIMIENTO DE COMBUSTIBLE", WingType = rotaryWing },
            new() { Code = 1262, Name = "PARTICIPAR EN UNA REVISION DESPUES DE LA ACCION A NIVEL TRIPULACION", WingType = rotaryWing },
            new() { Code = 1321, Name = "EJECUTAR FALLA SIMULADA ANTITORQUE", WingType = rotaryWing },
            new() { Code = 1323, Name = "EJECUTAR AUTORROTACIÓN EN ESTACIONARIO", WingType = rotaryWing },
            new() { Code = 1327, Name = "EJECUTAR AUTORROTACIÓN DE BAJO NIVEL", WingType = rotaryWing },
            new() { Code = 1335, Name = "EJECUTAR AUTORROTACIÓN ESTANDAR CON VIRAJE", WingType = rotaryWing },
            new() { Code = 1476, Name = "EJECUTAR INSPECCIÓN POST-VUELO", WingType = rotaryWing },
            new() { Code = 2010, Name = "EJECUTAR OPERACIONES CON MÚLTIPLES AERONAVES", WingType = rotaryWing },
            new() { Code = 2011, Name = "EJECUTAR ROMPIMIENTO SIMULADO DE FORMACION IIMC", WingType = rotaryWing },
            new() { Code = 2012, Name = "EJECUTAR PLANIAMIENTO TACTICO PARA MISIÓN DE VUELO", WingType = rotaryWing },
            new() { Code = 2024, Name = "EJECUTAR NAVEGACION DE VUELO SOBRE EL TERRENO", WingType = rotaryWing },
            new() { Code = 2025, Name = "EJECUTAR DESPEGUE DE VUELO SOBRE EL TERRENO", WingType = rotaryWing },
            new() { Code = 2026, Name = "EJECUTAR VUELO SOBRE EL TERRENO", WingType = rotaryWing },
            new() { Code = 2030, Name = "EJECUTAR APROXIMACIÓN DE VUELO SOBRE EL TERRENO", WingType = rotaryWing },
            new() { Code = 2034, Name = "EJECUTAR OCULTACIÓN Y EXPOSICIÓN", WingType = rotaryWing },
            new() { Code = 2036, Name = "EJECUTAR DESACELERACIÓN DE VUELO SOBRE EL TERRENO", WingType = rotaryWing },
            new() { Code = 2041, Name = "EJECUTAR RECONOCIMIENTO DE RUTA", WingType = rotaryWing },
            new() { Code = 2049, Name = "EJECUTAR AERONAVEGACIÓN CON AYUDA DEL GPS", WingType = rotaryWing },
            new() { Code = 2067, Name = "EJECUTAR RECONOCIMIENTO DE ZA/ZE Y AREA DE ESPERA", WingType = rotaryWing },
            new() { Code = 2125, Name = "EJECUTAR OPERACIONES DE PINACULO O SIERRA", WingType = rotaryWing },
            new() { Code = 2169, Name = "EJECUTAR OBSERVACION AEREA", WingType = rotaryWing },
            new() { Code = 3003, Name = "DOMINIO DEL SISTEMA HIDRÁULICO EN VUELO ESTACIONARIO", WingType = rotaryWing },
            new() { Code = 3004, Name = "EJECUTAR UNA FALLA ANTITORQUE SIMULADA EN VUELO ESTACIONARIO", WingType = rotaryWing },
            new() { Code = 3015, Name = "EJECUTAR AUTORROTACIÓN DURANTE EL RODAJE", WingType = rotaryWing },
            new() { Code = 3016, Name = "EJECUTAR APROXIMACIÓN DE PROFUNDIDAD", WingType = rotaryWing },
            new() { Code = 3019, Name = "EJECUTAR VIGILANCIA PATRULLAJE AÉREO", WingType = rotaryWing }
        ];
    }

    public static Dictionary<(int PhaseNumber, int MissionNumber), List<TaskMapping>> GetRotaryWingMappings()
    {
        var wingType = WingTypes.ROTARY_WING;

        return new Dictionary<(int PhaseNumber, int MissionNumber), List<TaskMapping>>
        {
            // FASE 1 DE PRE-SOLO (PhaseNumber: 1, Misiones 1-22)
            [(1, 1)] = [
                new(1111, false, wingType), new(1024, false, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, false, wingType), new(1004, false, wingType), new(1010, false, wingType), new(1012, false, wingType),
                new(1022, false, wingType), new(1026, false, wingType), new(1028, false, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, false, wingType), new(1190, false, wingType), new(1262, false, wingType), new(1476, false, wingType)
            ],
            [(1, 2)] = [
                new(1111, false, wingType), new(1024, false, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, false, wingType), new(1004, false, wingType), new(1010, false, wingType), new(1012, false, wingType),
                new(1022, false, wingType), new(1026, false, wingType), new(1028, false, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, false, wingType), new(1190, false, wingType), new(1262, false, wingType), new(1476, false, wingType)
            ],
            [(1, 3)] = [
                new(1111, false, wingType), new(1024, false, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, false, wingType), new(1004, false, wingType), new(1010, false, wingType), new(1012, false, wingType),
                new(1022, false, wingType), new(1026, false, wingType), new(1028, false, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, false, wingType), new(1190, false, wingType), new(1262, false, wingType), new(1476, false, wingType)
            ],
            [(1, 4)] = [
                new(1111, false, wingType), new(1024, false, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, false, wingType), new(1004, false, wingType), new(1010, false, wingType), new(1012, false, wingType),
                new(1022, false, wingType), new(1026, false, wingType), new(1028, false, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, false, wingType), new(1190, false, wingType), new(1262, false, wingType), new(1476, false, wingType)
            ],
            [(1, 5)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, false, wingType), new(1262, false, wingType), new(1476, false, wingType)
            ],
            [(1, 6)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, false, wingType), new(1262, false, wingType), new(1476, false, wingType)
            ],
            [(1, 7)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, false, wingType), new(1262, false, wingType), new(1476, false, wingType)
            ],
            [(1, 8)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 9)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, false, wingType), new(4444, false, wingType), new(5555, false, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, false, wingType), new(1038, false, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 10)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 11)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 12)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 13)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 14)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, false, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 15)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, true, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 16)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, true, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 17)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, true, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 18)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, false, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, false, wingType), new(1041, false, wingType), new(1048, true, wingType), new(1052, false, wingType), new(1068, false, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ], // repaso
            [(1, 19)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, true, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType), new(1052, true, wingType), new(1068, true, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ],
            [(1, 20)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, true, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType), new(1052, true, wingType), new(1068, true, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ], // repaso
            [(1, 21)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, true, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType), new(1052, true, wingType), new(1068, true, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ], // repaso
            [(1, 22)] = [
                new(1111, true, wingType), new(1024, true, wingType), new(3333, true, wingType), new(4444, true, wingType), new(5555, true, wingType),
                new(1058, true, wingType), new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType),
                new(1022, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType), new(1052, true, wingType), new(1068, true, wingType),
                new(1074, false, wingType), new(1194, true, wingType), new(1190, true, wingType), new(1262, true, wingType), new(1476, true, wingType)
            ], // repaso

            // FASE 2 DE VUELO BÁSICO (PhaseNumber: 2, Misiones 1-37)
            [(2, 1)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 2)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 3)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 4)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 5)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 6)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 7)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 8)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, false, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 9)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, false, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 10)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 11)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, false, wingType),
                new(3016, false, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 12)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 13)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, false, wingType), new(1076, false, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ], // repaso
            [(2, 14)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 15)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, false, wingType), new(1323, false, wingType),
                new(3015, false, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 16)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 17)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ], // repaso
            [(2, 18)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 19)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 20)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ], // repaso
            [(2, 21)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, false, wingType), new(1321, false, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ], // repaso
            [(2, 22)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, false, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 23)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, false, wingType),
                new(1327, false, wingType), new(1335, false, wingType), new(1068, false, wingType), new(1070, false, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 24)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 25)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ], // repaso
            [(2, 26)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ], // repaso
            [(2, 27)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 28)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 29)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 30)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 31)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, false, wingType),
                new(1062, false, wingType), new(1063, false, wingType), new(2125, false, wingType)
            ],
            [(2, 32)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, true, wingType),
                new(1062, true, wingType), new(1063, true, wingType), new(2125, true, wingType)
            ],
            [(2, 33)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, true, wingType),
                new(1062, true, wingType), new(1063, true, wingType), new(2125, true, wingType)
            ], // repaso
            [(2, 34)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, true, wingType),
                new(1062, true, wingType), new(1063, true, wingType), new(2125, true, wingType)
            ], // repaso
            [(2, 35)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, true, wingType),
                new(1062, true, wingType), new(1063, true, wingType), new(2125, true, wingType)
            ], // repaso
            [(2, 36)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, true, wingType),
                new(1062, true, wingType), new(1063, true, wingType), new(2125, true, wingType)
            ], // repaso
            [(2, 37)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1022, true, wingType),
                new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1032, true, wingType), new(1038, true, wingType),
                new(1040, true, wingType), new(1041, true, wingType), new(1058, true, wingType), new(1048, true, wingType), new(1194, true, wingType),
                new(1190, true, wingType), new(1476, true, wingType), new(1052, true, wingType), new(1066, true, wingType), new(1031, true, wingType),
                new(3016, true, wingType), new(3003, true, wingType), new(1076, true, wingType), new(1072, true, wingType), new(1323, true, wingType),
                new(3015, true, wingType), new(3004, true, wingType), new(1321, true, wingType), new(1074, true, wingType), new(1082, true, wingType),
                new(1327, true, wingType), new(1335, true, wingType), new(1068, true, wingType), new(1070, true, wingType), new(1030, true, wingType),
                new(1062, true, wingType), new(1063, true, wingType), new(2125, true, wingType)
            ], // evaluación

            // FASE 3 DE VUELO TÁCTICO (PhaseNumber: 3, Misiones 1-8)
            [(3, 1)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, false, wingType), new(1155, false, wingType), new(2036, false, wingType), new(6666, false, wingType),
                new(2025, false, wingType), new(2034, false, wingType), new(2010, false, wingType), new(2030, false, wingType), new(2169, false, wingType),
                new(2024, false, wingType), new(2041, false, wingType), new(1044, false, wingType), new(7777, false, wingType), new(2049, false, wingType),
                new(3019, false, wingType)
            ],
            [(3, 2)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, false, wingType), new(1155, false, wingType), new(2036, false, wingType), new(6666, false, wingType),
                new(2025, false, wingType), new(2034, false, wingType), new(2010, false, wingType), new(2030, false, wingType), new(2169, false, wingType),
                new(2024, false, wingType), new(2041, false, wingType), new(1044, false, wingType), new(7777, false, wingType), new(2049, false, wingType),
                new(3019, false, wingType)
            ],
            [(3, 3)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, true, wingType), new(1155, true, wingType), new(2036, true, wingType), new(6666, true, wingType),
                new(2025, true, wingType), new(2034, true, wingType), new(2010, false, wingType), new(2030, false, wingType), new(2169, false, wingType),
                new(2024, false, wingType), new(2041, false, wingType), new(1044, false, wingType), new(7777, false, wingType), new(2049, false, wingType),
                new(3019, false, wingType)
            ],
            [(3, 4)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, true, wingType), new(1155, true, wingType), new(2036, true, wingType), new(6666, true, wingType),
                new(2025, true, wingType), new(2034, true, wingType), new(2010, false, wingType), new(2030, false, wingType), new(2169, false, wingType),
                new(2024, false, wingType), new(2041, false, wingType), new(1044, false, wingType), new(7777, false, wingType), new(2049, false, wingType),
                new(3019, false, wingType)
            ],
            [(3, 5)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, true, wingType), new(1155, true, wingType), new(2036, true, wingType), new(6666, true, wingType),
                new(2025, true, wingType), new(2034, true, wingType), new(2010, true, wingType), new(2030, true, wingType), new(2169, false, wingType),
                new(2024, false, wingType), new(2041, false, wingType), new(1044, false, wingType), new(7777, false, wingType), new(2049, false, wingType),
                new(3019, false, wingType)
            ],
            [(3, 6)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, true, wingType), new(1155, true, wingType), new(2036, true, wingType), new(6666, true, wingType),
                new(2025, true, wingType), new(2034, true, wingType), new(2010, true, wingType), new(2030, true, wingType), new(2169, true, wingType),
                new(2024, true, wingType), new(2041, true, wingType), new(1044, false, wingType), new(7777, false, wingType), new(2049, false, wingType),
                new(3019, false, wingType)
            ],
            [(3, 7)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, true, wingType), new(1155, true, wingType), new(2036, true, wingType), new(6666, true, wingType),
                new(2025, true, wingType), new(2034, true, wingType), new(2010, true, wingType), new(2030, true, wingType), new(2169, true, wingType),
                new(2024, true, wingType), new(2041, true, wingType), new(1044, true, wingType), new(7777, false, wingType), new(2049, false, wingType),
                new(3019, false, wingType)
            ],
            [(3, 8)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1010, true, wingType), new(1012, true, wingType), new(1014, true, wingType),
                new(1022, true, wingType), new(1024, true, wingType), new(1026, true, wingType), new(1028, true, wingType), new(1030, true, wingType),
                new(1032, true, wingType), new(1038, true, wingType), new(1040, true, wingType), new(1041, true, wingType), new(1048, true, wingType),
                new(1052, true, wingType), new(1058, true, wingType), new(1062, true, wingType), new(1063, true, wingType), new(1476, true, wingType),
                new(2125, true, wingType), new(2026, true, wingType), new(1155, true, wingType), new(2036, true, wingType), new(6666, true, wingType),
                new(2025, true, wingType), new(2034, true, wingType), new(2010, true, wingType), new(2030, true, wingType), new(2169, true, wingType),
                new(2024, true, wingType), new(2041, true, wingType), new(1044, true, wingType), new(7777, true, wingType), new(2049, true, wingType),
                new(3019, true, wingType)
            ],

            // FASE 4 DE INSTRUMENTOS (PhaseNumber: 4, Misiones 1-20)
            [(4, 1)] = [
                new(1111, false, wingType), new(1000, false, wingType), new(1006, false, wingType), new(1010, false, wingType), new(1032, false, wingType),
                new(1048, false, wingType), new(8888, false, wingType), new(9000, false, wingType), new(9001, false, wingType), new(9002, false, wingType),
                new(9003, false, wingType), new(9004, false, wingType), new(9005, false, wingType), new(9006, false, wingType), new(9007, false, wingType),
                new(9008, false, wingType), new(1170, false, wingType), new(1182, false, wingType), new(1184, false, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 2)] = [
                new(1111, false, wingType), new(1000, false, wingType), new(1006, false, wingType), new(1010, false, wingType), new(1032, false, wingType),
                new(1048, false, wingType), new(8888, false, wingType), new(9000, false, wingType), new(9001, false, wingType), new(9002, false, wingType),
                new(9003, false, wingType), new(9004, false, wingType), new(9005, false, wingType), new(9006, false, wingType), new(9007, false, wingType),
                new(9008, false, wingType), new(1170, false, wingType), new(1182, false, wingType), new(1184, false, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 3)] = [
                new(1111, false, wingType), new(1000, false, wingType), new(1006, false, wingType), new(1010, false, wingType), new(1032, false, wingType),
                new(1048, false, wingType), new(8888, false, wingType), new(9000, false, wingType), new(9001, false, wingType), new(9002, false, wingType),
                new(9003, false, wingType), new(9004, false, wingType), new(9005, false, wingType), new(9006, false, wingType), new(9007, false, wingType),
                new(9008, false, wingType), new(1170, false, wingType), new(1182, false, wingType), new(1184, false, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 4)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, false, wingType), new(9000, false, wingType), new(9001, false, wingType), new(9002, false, wingType),
                new(9003, false, wingType), new(9004, false, wingType), new(9005, false, wingType), new(9006, false, wingType), new(9007, false, wingType),
                new(9008, false, wingType), new(1170, false, wingType), new(1182, false, wingType), new(1184, false, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 5)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, false, wingType), new(9000, false, wingType), new(9001, false, wingType), new(9002, false, wingType),
                new(9003, false, wingType), new(9004, false, wingType), new(9005, false, wingType), new(9006, false, wingType), new(9007, false, wingType),
                new(9008, false, wingType), new(1170, false, wingType), new(1182, false, wingType), new(1184, false, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 6)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, false, wingType), new(9000, false, wingType), new(9001, false, wingType), new(9002, false, wingType),
                new(9003, false, wingType), new(9004, false, wingType), new(9005, false, wingType), new(9006, false, wingType), new(9007, false, wingType),
                new(9008, false, wingType), new(1170, false, wingType), new(1182, false, wingType), new(1184, false, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 7)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 8)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, false, wingType),
                new(9009, false, wingType), new(9010, false, wingType), new(9011, false, wingType), new(9012, false, wingType), new(9013, false, wingType),
                new(9014, false, wingType), new(9015, false, wingType), new(9016, false, wingType), new(9017, false, wingType), new(9018, false, wingType),
                new(9019, false, wingType), new(9020, false, wingType), new(9021, false, wingType), new(1174, false, wingType), new(9022, false, wingType),
                new(9023, false, wingType), new(9024, false, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 9)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 10)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 11)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, false, wingType), new(9025, false, wingType), new(1080, false, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 12)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 13)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 14)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 15)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, false, wingType), new(9026, false, wingType), new(9027, false, wingType), new(9028, false, wingType), new(9029, false, wingType),
                new(9030, false, wingType), new(9031, false, wingType), new(9032, false, wingType), new(9033, false, wingType)
            ],
            [(4, 16)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, true, wingType), new(9026, true, wingType), new(9027, true, wingType), new(9028, true, wingType), new(9029, true, wingType),
                new(9030, true, wingType), new(9031, true, wingType), new(9032, true, wingType), new(9033, true, wingType)
            ],
            [(4, 17)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, true, wingType), new(9026, true, wingType), new(9027, true, wingType), new(9028, true, wingType), new(9029, true, wingType),
                new(9030, true, wingType), new(9031, true, wingType), new(9032, true, wingType), new(9033, true, wingType)
            ], // repaso
            [(4, 18)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, true, wingType), new(9026, true, wingType), new(9027, true, wingType), new(9028, true, wingType), new(9029, true, wingType),
                new(9030, true, wingType), new(9031, true, wingType), new(9032, true, wingType), new(9033, true, wingType)
            ], // repaso
            [(4, 19)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, true, wingType), new(9026, true, wingType), new(9027, true, wingType), new(9028, true, wingType), new(9029, true, wingType),
                new(9030, true, wingType), new(9031, true, wingType), new(9032, true, wingType), new(9033, true, wingType)
            ], // repaso
            [(4, 20)] = [
                new(1111, true, wingType), new(1000, true, wingType), new(1006, true, wingType), new(1010, true, wingType), new(1032, true, wingType),
                new(1048, true, wingType), new(8888, true, wingType), new(9000, true, wingType), new(9001, true, wingType), new(9002, true, wingType),
                new(9003, true, wingType), new(9004, true, wingType), new(9005, true, wingType), new(9006, true, wingType), new(9007, true, wingType),
                new(9008, true, wingType), new(1170, true, wingType), new(1182, true, wingType), new(1184, true, wingType), new(1172, true, wingType),
                new(9009, true, wingType), new(9010, true, wingType), new(9011, true, wingType), new(9012, true, wingType), new(9013, true, wingType),
                new(9014, true, wingType), new(9015, true, wingType), new(9016, true, wingType), new(9017, true, wingType), new(9018, true, wingType),
                new(9019, true, wingType), new(9020, true, wingType), new(9021, true, wingType), new(1174, true, wingType), new(9022, true, wingType),
                new(9023, true, wingType), new(9024, true, wingType), new(1176, true, wingType), new(9025, true, wingType), new(1080, true, wingType),
                new(1178, true, wingType), new(9026, true, wingType), new(9027, true, wingType), new(9028, true, wingType), new(9029, true, wingType),
                new(9030, true, wingType), new(9031, true, wingType), new(9032, true, wingType), new(9033, true, wingType)
            ], // repaso
            // FASE 5 DE CRUCEROS (PhaseNumber: 5, Misiones 1-3)
            [(5, 1)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1012, true, wingType), new(1022, true, wingType), new(1024, true, wingType),
                new(1014, true, wingType), new(1028, true, wingType), new(1038, true, wingType), new(1048, true, wingType), new(1040, true, wingType),
                new(1044, true, wingType), new(1058, true, wingType), new(1476, true, wingType), new(1002, true, wingType), new(1172, true, wingType),
                new(1174, true, wingType), new(1032, true, wingType), new(1176, true, wingType), new(2010, true, wingType), new(2026, true, wingType)

            ],
            [(5, 2)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1012, true, wingType), new(1022, true, wingType), new(1024, true, wingType),
                new(1014, true, wingType), new(1028, true, wingType), new(1038, true, wingType), new(1048, true, wingType), new(1040, true, wingType),
                new(1044, true, wingType), new(1058, true, wingType), new(1476, true, wingType), new(1002, true, wingType), new(1172, true, wingType),
                new(1174, true, wingType), new(1032, true, wingType), new(1176, true, wingType), new(2010, true, wingType), new(2026, true, wingType)
            ],
            [(5, 3)] = [
                new(1000, true, wingType), new(1004, true, wingType), new(1012, true, wingType), new(1022, true, wingType), new(1024, true, wingType),
                new(1014, true, wingType), new(1028, true, wingType), new(1038, true, wingType), new(1048, true, wingType), new(1040, true, wingType),
                new(1044, true, wingType), new(1058, true, wingType), new(1476, true, wingType), new(1002, true, wingType), new(1172, true, wingType),
                new(1174, true, wingType), new(1032, true, wingType), new(1176, true, wingType), new(2010, true, wingType), new(2026, true, wingType)
            ],
        };
    }

}