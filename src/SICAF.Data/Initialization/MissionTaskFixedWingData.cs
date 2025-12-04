using SICAF.Data.Entities.Academic;

using static SICAF.Common.Constants.AviationConstants;

namespace SICAF.Data.Initialization;

public static class MissionTaskFixedWingData
{
    public static List<Tasks> GetFixedWingTasks()
    {
        string fixedWing = WingTypes.FIXED_WING;
        return
        [
            // Tareas Serie 1000 - Tareas básicas de entrenamiento
            new() { Code = 1000, Name = "ORIENTACIÓN DE LA TRIPULACIÓN", WingType = fixedWing },
            new() { Code = 1001, Name = "EJECUTAR PLANIFICACIÓN VUELO VFR", WingType = fixedWing },
            new() { Code = 1002, Name = "EJECUTAR PLANIFICACIÓN VUELO IFR", WingType = fixedWing },
            new() { Code = 1003, Name = "EJECUTAR DILIGENCIAMIENTO DEL FORMATO DE PESO Y BALANCE", WingType = fixedWing },
            new() { Code = 1005, Name = "EJECUTAR INSPECCIÓN PRE-VUELO", WingType = fixedWing },
            new() { Code = 1007, Name = "EJECUTAR RODAJE DEL AVIÓN", WingType = fixedWing },
            new() { Code = 1008, Name = "EJECUTAR ENCENDIDO DEL MOTOR, CALENTAMIENTO Y PRUEBAS", WingType = fixedWing },
            new() { Code = 1009, Name = "EJECUTAR DESPEGUE Y ASCENSO NORMAL", WingType = fixedWing },
            new() { Code = 1010, Name = "EJECUTAR VUELO RECTO Y A NIVEL", WingType = fixedWing },
            new() { Code = 1011, Name = "EJECUTAR ASCENSOS Y DESCENSOS", WingType = fixedWing },
            new() { Code = 1012, Name = "EJECUTAR VIRAJES", WingType = fixedWing },
            new() { Code = 1013, Name = "EJECUTAR VUELO LENTO (VMCA)", WingType = fixedWing },
            new() { Code = 1014, Name = "EJECUTAR Y RECOBRAR PÉRDIDAS", WingType = fixedWing },
            new() { Code = 1016, Name = "EJECUTAR NAVEGACIÓN A LA ESTIMA", WingType = fixedWing },
            new() { Code = 1017, Name = "EJECUTAR OPERACIÓN Y USO DE LOS CONTROLES DE VUELO, COMPENSADOR Y FLAPS", WingType = fixedWing },
            new() { Code = 1018, Name = "EJECUTAR ATERRIZAJES DE TOQUE Y DESPEGUE", WingType = fixedWing },
            new() { Code = 1019, Name = "EJECUTAR DESPEGUE DE MÁXIMO RENDIMIENTO", WingType = fixedWing },
            new() { Code = 1020, Name = "EJECUTAR ATERRIZAJES CORTOS", WingType = fixedWing },
            new() { Code = 1021, Name = "EJECUTAR ATERRIZAJE CON VIENTO CRUZADO", WingType = fixedWing },
            new() { Code = 1022, Name = "EJECUTAR PROCEDIMIENTO DE MOTOR Y AL AIRE", WingType = fixedWing },
            new() { Code = 1023, Name = "EJECUTAR CIRCUITO DE TRÁNSITO NORMAL", WingType = fixedWing },
            new() { Code = 1024, Name = "EJECUTAR PROCEDIMIENTO POR CAMBIO DE PISTA EN USO", WingType = fixedWing },
            new() { Code = 1025, Name = "EJECUTAR PROCEDIMIENTOS RADIOTELEFÓNICOS", WingType = fixedWing },
            new() { Code = 1026, Name = "EJECUTAR PROCEDIMIENTOS DE ENTRADA Y SALIDA DE LAS ZONAS DE ENTRENAMIENTO", WingType = fixedWing },
            new() { Code = 1027, Name = "EJECUTAR CORRECTAMENTE EL USO DE LAS LISTAS DE CHEQUEO", WingType = fixedWing },
            new() { Code = 1028, Name = "EJECUTAR USO ADECUADO DE LOS FRENOS, PARQUEO Y APAGADO DEL AVIÓN", WingType = fixedWing },
            new() { Code = 1029, Name = "EJECUTAR PROCEDIMIENTO DE ATERRIZAJE CON DIFERENTES SEGMENTOS DE FLAPS", WingType = fixedWing },
            new() { Code = 1030, Name = "EJECUTAR PROCEDIMIENTOS DE EMERGENCIA", WingType = fixedWing },

            // Tareas Serie 1000 - Instrumentos
            new() { Code = 1040, Name = "EJECUTAR DESPEGUE POR INSTRUMENTOS", WingType = fixedWing },
            new() { Code = 1041, Name = "EJECUTAR ASCENSOS, DESCENSOS Y VUELO RECTO Y NIVELADO POR INSTRUMENTOS", WingType = fixedWing },
            new() { Code = 1042, Name = "EJECUTAR VIRAJES A NIVEL, CON VELOCIDAD CONSTANTE A RUMBO PREDETERMINADO, POR TIEMPO Y CON PANEL PARCIAL", WingType = fixedWing },
            new() { Code = 1043, Name = "EJECUTAR PROCEDIMIENTOS VOR", WingType = fixedWing },
            new() { Code = 1044, Name = "EJECUTAR PROCEDIMIENTOS DE ESPERA", WingType = fixedWing },
            new() { Code = 1045, Name = "EJECUTAR RECOBRO DE POSICIONES ANORMALES", WingType = fixedWing },
            new() { Code = 1046, Name = "EJECUTAR DEMOSTRACIÓN DE ERRORES DE LA BRÚJULA", WingType = fixedWing },
            new() { Code = 1047, Name = "EJECUTAR O DESCRIBIR PROCEDIMIENTOS EN CASO DE FALLAS EN LAS COMUNICACIONES", WingType = fixedWing },
            new() { Code = 1048, Name = "EJECUTAR UNA APROXIMACIÓN DE NO PRECISIÓN", WingType = fixedWing },
            new() { Code = 1049, Name = "EJECUTAR UNA APROXIMACIÓN DE PRECISIÓN", WingType = fixedWing },
            new() { Code = 1050, Name = "EJECUTAR UNA APROXIMACIÓN FRUSTRADA", WingType = fixedWing },
            new() { Code = 1051, Name = "EJECUTAR \"S\" 1-2-3 Y 4 NORMAL Y CON PANEL PARCIAL", WingType = fixedWing },
            new() { Code = 1052, Name = "EJECUTAR LA NAVEGACIÓN CON EL GNSS", WingType = fixedWing },
            new() { Code = 1057, Name = "EJECUTAR FALLA SIMULADA DE LOS CONTROLES DE VUELO", WingType = fixedWing },
            new() { Code = 1059, Name = "EJECUTAR PROCEDIMIENTO ARCOS DME", WingType = fixedWing },

            // Tareas Serie 2000 - Tareas específicas y operacionales
            new() { Code = 2001, Name = "EJECUTAR ATERRIZAJES EN PISTAS NO CONTROLADAS", WingType = fixedWing },
            new() { Code = 2006, Name = "EJECUTAR PRECISIÓN 180°", WingType = fixedWing },
            new() { Code = 2007, Name = "EJECUTAR PRECISIÓN 360°", WingType = fixedWing },
            new() { Code = 2008, Name = "EJECUTAR CIRCUITO DE TRÁNSITO 400X1", WingType = fixedWing },
            new() { Code = 2014, Name = "CONOCIMIENTOS BÁSICOS DE AVIACIÓN Y GENERALIDADES DE LA AERONAVE", WingType = fixedWing },
            new() { Code = 2018, Name = "EJECUTAR CHANDELLES", WingType = fixedWing },
            new() { Code = 2019, Name = "EJECUTAR MEDIAS VUELTAS", WingType = fixedWing },
            new() { Code = 2020, Name = "EJECUTAR OCHOS SOBRE EL HORIZONTE", WingType = fixedWing }
        ];
    }

    public static Dictionary<(int PhaseNumber, int MissionNumber), List<TaskMapping>> GetFixedWingMappings()
    {
        var wingType = WingTypes.FIXED_WING;

        return new Dictionary<(int PhaseNumber, int MissionNumber), List<TaskMapping>>
        {
            // FASE 1 DE PRE-SOLO (PhaseNumber: 1, Misiones 1-21)
            [(1, 1)] = [
                new(1000, false, wingType), new(1003, false, wingType), new(1005, false, wingType), new(1007, false, wingType), new(1008, false, wingType),
                new(1009, false, wingType), new(1010, false, wingType), new(1011, false, wingType), new(1012, false, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, false, wingType), new(1027, false, wingType),
                new(1028, false, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, false, wingType), new(2014, false, wingType)
            ],
            [(1, 2)] = [
                new(1000, false, wingType), new(1003, false, wingType), new(1005, false, wingType), new(1007, false, wingType), new(1008, false, wingType),
                new(1009, false, wingType), new(1010, false, wingType), new(1011, false, wingType), new(1012, false, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, false, wingType), new(1027, false, wingType),
                new(1028, false, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, false, wingType), new(2014, false, wingType)
            ],
            [(1, 3)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, false, wingType), new(1007, false, wingType), new(1008, false, wingType),
                new(1009, false, wingType), new(1010, false, wingType), new(1011, false, wingType), new(1012, false, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, false, wingType), new(1027, false, wingType),
                new(1028, false, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, false, wingType), new(2014, false, wingType)
            ],
            [(1, 4)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, false, wingType), new(1008, true, wingType),
                new(1009, false, wingType), new(1010, false, wingType), new(1011, false, wingType), new(1012, false, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, false, wingType), new(1027, false, wingType),
                new(1028, false, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, false, wingType), new(2014, false, wingType)
            ],
            [(1, 5)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, false, wingType), new(1010, false, wingType), new(1011, false, wingType), new(1012, false, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, false, wingType), new(1027, false, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, false, wingType), new(2014, false, wingType)
            ],
            [(1, 6)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, false, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, false, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, false, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, false, wingType), new(2014, false, wingType)
            ],
            [(1, 7)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, false, wingType), new(2014, false, wingType)
            ],
            [(1, 8)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, false, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, true, wingType), new(2014, false, wingType)
            ],
            [(1, 9)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, true, wingType), new(2014, false, wingType)
            ],
            [(1, 10)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, false, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 11)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, false, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 12)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, false, wingType),
                new(1023, false, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 13)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, true, wingType),
                new(1023, false, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, false, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 14)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, true, wingType),
                new(1023, false, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 15)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, true, wingType),
                new(1023, true, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, false, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 16)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, true, wingType),
                new(1023, true, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, true, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 17)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, false, wingType), new(1022, true, wingType),
                new(1023, true, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, true, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 18)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, true, wingType), new(1022, true, wingType),
                new(1023, true, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, true, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 19)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1021, true, wingType), new(1022, true, wingType),
                new(1023, true, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, true, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 20)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, true, wingType), new(1021, true, wingType), new(1022, true, wingType),
                new(1023, true, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, true, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],
            [(1, 21)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1017, true, wingType), new(1018, true, wingType), new(1021, true, wingType), new(1022, true, wingType),
                new(1023, true, wingType), new(1024, true, wingType), new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType),
                new(1028, true, wingType), new(1029, true, wingType), new(1030, true, wingType), new(1045, true, wingType), new(2014, true, wingType)
            ],

            // FASE 2 DE MANIOBRAS (PhaseNumber: 2, Misiones 1-40)
            [(2, 1)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, false, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, false, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 2)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, false, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, false, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 3)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, false, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, false, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 4)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, false, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, false, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 5)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, false, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, false, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 6)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 7)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 8)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 9)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 10)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 11)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 12)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 13)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 14)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 15)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, false, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 16)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 17)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, false, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 18)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 19)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, false, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, false, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 20)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 21)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, false, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 22)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, true, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 23)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, true, wingType), new(2007, false, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 24)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 25)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, false, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 26)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 27)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 28)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, false, wingType),
                new(1014, false, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, false, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 29)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 30)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, false, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 31)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, false, wingType), new(2020, false, wingType)
            ],
            [(2, 32)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, false, wingType)
            ],
            [(2, 33)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],
            [(2, 34)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],
            [(2, 35)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, false, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],
            [(2, 36)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, true, wingType), new(1019, false, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],
            [(2, 37)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, true, wingType), new(1019, true, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],
            [(2, 38)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, true, wingType), new(1019, true, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],
            [(2, 39)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, true, wingType), new(1019, true, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],
            [(2, 40)] = [
                new(1000, true, wingType), new(1003, true, wingType), new(1005, true, wingType), new(1007, true, wingType), new(1008, true, wingType),
                new(1009, true, wingType), new(1010, true, wingType), new(1011, true, wingType), new(1012, true, wingType), new(1013, true, wingType),
                new(1014, true, wingType), new(1016, true, wingType), new(1017, true, wingType), new(1018, true, wingType), new(1019, true, wingType),
                new(1020, true, wingType), new(1021, true, wingType), new(1022, true, wingType), new(1023, true, wingType), new(1024, true, wingType),
                new(1025, true, wingType), new(1026, true, wingType), new(1027, true, wingType), new(1028, true, wingType), new(1029, true, wingType),
                new(1030, true, wingType), new(1057, true, wingType), new(2006, true, wingType), new(2007, true, wingType), new(2008, true, wingType),
                new(2014, true, wingType), new(2018, true, wingType), new(2019, true, wingType), new(2020, true, wingType)
            ],

            // FASE 3 DE INSTRUMENTOS (PhaseNumber: 3, Misiones 1-19)
            [(3, 1)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, false, wingType), new(1041, false, wingType),
                new(1042, false, wingType), new(1043, false, wingType), new(1044, false, wingType), new(1045, false, wingType), new(1046, false, wingType),
                new(1047, false, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, false, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 2)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, false, wingType), new(1044, false, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, false, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, false, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 3)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, false, wingType), new(1044, false, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, false, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, false, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 4)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, false, wingType), new(1044, false, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 5)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, false, wingType), new(1044, false, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 6)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, false, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 7)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, false, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 8)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, false, wingType), new(2014, true, wingType)
            ],
            [(3, 9)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 10)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 11)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 12)] = [
                new(1000, true, wingType), new(1002, false, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, false, wingType), new(1049, false, wingType), new(1050, false, wingType), new(1051, true, wingType),
                new(1052, false, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 13)] = [
                new(1000, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, true, wingType), new(1049, false, wingType), new(1050, true, wingType), new(1051, true, wingType),
                new(1052, true, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 14)] = [
                new(1000, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, true, wingType), new(1049, false, wingType), new(1050, true, wingType), new(1051, true, wingType),
                new(1052, true, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 15)] = [
                new(1000, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, true, wingType), new(1049, false, wingType), new(1050, true, wingType), new(1051, true, wingType),
                new(1052, true, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 16)] = [
                new(1000, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, true, wingType), new(1049, false, wingType), new(1050, true, wingType), new(1051, true, wingType),
                new(1052, true, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 17)] = [
                new(1000, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, true, wingType), new(1049, true, wingType), new(1050, true, wingType), new(1051, true, wingType),
                new(1052, true, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 18)] = [
                new(1000, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, true, wingType), new(1049, true, wingType), new(1050, true, wingType), new(1051, true, wingType),
                new(1052, true, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],
            [(3, 19)] = [
                new(1000, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1040, true, wingType), new(1041, true, wingType),
                new(1042, true, wingType), new(1043, true, wingType), new(1044, true, wingType), new(1045, true, wingType), new(1046, true, wingType),
                new(1047, true, wingType), new(1048, true, wingType), new(1049, true, wingType), new(1050, true, wingType), new(1051, true, wingType),
                new(1052, true, wingType), new(1059, true, wingType), new(2014, true, wingType)
            ],

            // FASE 4 DE CRUCEROS (PhaseNumber: 4, Misiones 1-5) - Todas las misiones son P3
            [(4, 1)] = [
                new(1000, true, wingType), new(1001, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1025, true, wingType),
                new(1027, true, wingType), new(1049, true, wingType), new(1052, true, wingType), new(2001, true, wingType), new(2014, true, wingType)
            ],
            [(4, 2)] = [
                new(1000, true, wingType), new(1001, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1025, true, wingType),
                new(1027, true, wingType), new(1049, true, wingType), new(1052, true, wingType), new(2001, true, wingType), new(2014, true, wingType)
            ],
            [(4, 3)] = [
                new(1000, true, wingType), new(1001, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1025, true, wingType),
                new(1027, true, wingType), new(1049, true, wingType), new(1052, true, wingType), new(2001, true, wingType), new(2014, true, wingType)
            ],
            [(4, 4)] = [
                new(1000, true, wingType), new(1001, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1025, true, wingType),
                new(1027, true, wingType), new(1049, true, wingType), new(1052, true, wingType), new(2001, true, wingType), new(2014, true, wingType)
            ],
            [(4, 5)] = [
                new(1000, true, wingType), new(1001, true, wingType), new(1002, true, wingType), new(1003, true, wingType), new(1025, true, wingType),
                new(1027, true, wingType), new(1049, true, wingType), new(1052, true, wingType), new(2001, true, wingType), new(2014, true, wingType)
            ]
        };
    }

}

public class TaskMapping(int taskCode, bool isP3, string wingType, int order = 0)
{
    public int TaskCode { get; } = taskCode;
    public bool IsP3 { get; } = isP3;
    public string WingType { get; } = wingType;
    public int DisplayOrder { get; } = order;
}