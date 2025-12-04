using Microsoft.Extensions.Logging;

using SICAF.Common.Constants;
using SICAF.Data.Entities.Catalogs;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

using static SICAF.Common.Constants.CatalogConstants;

namespace SICAF.Data.Initialization.Seeders;

public class MasterCatalogSeeder(IUnitOfWork unitOfWork, ILogger<MasterCatalogSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<MasterCatalogSeeder> _logger = logger;
    public int Priority => 3;

    public async Task SeedAsync()
    {
        try
        {
            // Verificar si ya existen datos
            if (await _unitOfWork.Repository<MasterCatalog>().AnyAsync())
            {
                _logger.LogInformation("Los catálogos ya contienen datos.");
                return;
            }

            var catalogs = new List<MasterCatalog>();

            // TIPOS DE DOCUMENTO
            catalogs.AddRange(GetDocumentTypes());

            // GRADOS MILITARES
            catalogs.AddRange(GetMilitaryGrades());

            // FUERZAS
            catalogs.AddRange(GetForces());

            // NACIONALIDADES
            catalogs.AddRange(GetNationalities());

            // TIPOS DE SANGRE
            catalogs.AddRange(GetBloodTypes());

            // POSICIONES DE VUELO
            catalogs.AddRange(GetFlightPositions());

            // Insertar todos los catálogos
            await _unitOfWork.Repository<MasterCatalog>().AddRangeAsync(catalogs);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Se insertaron {Count} registros en los catálogos.", catalogs.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar el seeder de catálogos.");
            throw;
        }
    }

    private static List<MasterCatalog> GetDocumentTypes()
    {
        return
            [
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.DOCUMENT_TYPE,
                    Code = "CC",
                    Name = "Cédula de Ciudadanía",
                    Description = "Documento de identidad para ciudadanos colombianos",
                    DisplayOrder = 1,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.DOCUMENT_TYPE,
                    Code = "CE",
                    Name = "Cédula de Extranjería",
                    Description = "Documento de identidad para extranjeros residentes",
                    DisplayOrder = 2,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.DOCUMENT_TYPE,
                    Code = "PAS",
                    Name = "Pasaporte",
                    Description = "Documento de viaje internacional",
                    DisplayOrder = 3,
                    CreatedBy = "System"
                }
            ];
    }

    private static List<MasterCatalog> GetMilitaryGrades()
    {
        var grades = new List<MasterCatalog>
        {
            // Oficiales Superiores
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "CR",
                Name = "Coronel",
                Description = "Oficial Superior",
                DisplayOrder = 1,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "TC",
                Name = "Teniente Coronel",
                Description = "Oficial Superior",
                DisplayOrder = 2,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "MY",
                Name = "Mayor",
                Description = "Oficial Superior",
                DisplayOrder = 3,
                CreatedBy = "System"
            },

            // Oficiales Subalternos
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "CT",
                Name = "Capitán",
                Description = "Oficial Subalterno",
                DisplayOrder = 4,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "TE",
                Name = "Teniente",
                Description = "Oficial Subalterno",
                DisplayOrder = 5,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "ST",
                Name = "Subteniente",
                Description = "Oficial Subalterno",
                DisplayOrder = 6,
                CreatedBy = "System"
            },

            // Nivel Ejecutivo
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "CM",
                Name = "Comisario",
                Description = "Nivel Ejecutivo",
                DisplayOrder = 7,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "SC",
                Name = "Subcomisario",
                Description = "Nivel Ejecutivo",
                DisplayOrder = 8,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "IJ",
                Name = "Intendente Jefe",
                Description = "Nivel Ejecutivo",
                DisplayOrder = 9,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "IT",
                Name = "Intendente",
                Description = "Nivel Ejecutivo",
                DisplayOrder = 10,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "SI",
                Name = "Subintendente",
                Description = "Nivel Ejecutivo",
                DisplayOrder = 11,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "PT",
                Name = "Patrullero",
                Description = "Nivel Ejecutivo",
                DisplayOrder = 12,
                CreatedBy = "System"
            },
            new() {
                CatalogType = CatalogTypes.MILITARY_GRADE,
                Code = "NU",
                Name = "No Uniformado",
                Description = "Civiles",
                DisplayOrder = 13,
                CreatedBy = "System"
            },
        };

        return grades;
    }

    private static List<MasterCatalog> GetForces()
    {
        return
            [
                new ()
                {
                    CatalogType = CatalogTypes.FORCE,
                    Code = "PONAL",
                    Name = "Policía Nacional",
                    Description = "Policía Nacional de Colombia",
                    DisplayOrder = 1,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.FORCE,
                    Code = "EJERCOL",
                    Name = "Ejército Nacional",
                    Description = "Ejército Nacional de Colombia",
                    DisplayOrder = 2,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.FORCE,
                    Code = "FAC",
                    Name = "Fuerza Aeroespacial Colombiana",
                    Description = "Fuerza Aérea de Colombia",
                    DisplayOrder = 3,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.FORCE,
                    Code = "ARC",
                    Name = "Armada Nacional",
                    Description = "Armada Nacional de Colombia",
                    DisplayOrder = 4,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.FORCE,
                    Code = "FEX",
                    Name = "Fuerza Extranjera",
                    Description = "Fuerza de un País Extranjero",
                    DisplayOrder = 5,
                    CreatedBy = "System"
                }
            ];
    }

    private static List<MasterCatalog> GetNationalities()
    {
        return
            [
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "COL",
                    Name = "Colombiana",
                    Description = "Nacionalidad Colombiana",
                    DisplayOrder = 1,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "CHL",
                    Name = "Chilena",
                    Description = "Nacionalidad Chilena",
                    DisplayOrder = 2,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "ECU",
                    Name = "Ecuatoriana",
                    Description = "Nacionalidad Ecuatoriana",
                    DisplayOrder = 3,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "PER",
                    Name = "Peruana",
                    Description = "Nacionalidad Peruana",
                    DisplayOrder = 4,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "BRA",
                    Name = "Brasileña",
                    Description = "Nacionalidad Brasileña",
                    DisplayOrder = 5,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "BOL",
                    Name = "Boliviana",
                    Description = "Nacionalidad Boliviana",
                    DisplayOrder = 6,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "PRY",
                    Name = "Paraguaya",
                    Description = "Nacionalidad Paraguaya",
                    DisplayOrder = 7,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "PAN",
                    Name = "Panameña",
                    Description = "Nacionalidad Panameña",
                    DisplayOrder = 8,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.NATIONALITY,
                    Code = "USA",
                    Name = "Estadounidense",
                    Description = "Nacionalidad Estadounidense",
                    DisplayOrder = 9,
                    CreatedBy = "System"
                },
            ];
    }

    private static List<MasterCatalog> GetBloodTypes()
    {
        return
            [
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "O+",
                    Name = "O Positivo",
                    DisplayOrder = 1,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "O-",
                    Name = "O Negativo",
                    DisplayOrder = 2,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "A+",
                    Name = "A Positivo",
                    DisplayOrder = 3,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "A-",
                    Name = "A Negativo",
                    DisplayOrder = 4,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "B+",
                    Name = "B Positivo",
                    DisplayOrder = 5,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "B-",
                    Name = "B Negativo",
                    DisplayOrder = 6,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "AB+",
                    Name = "AB Positivo",
                    DisplayOrder = 7,
                    CreatedBy = "System"
                },
                new ()
                {
                    CatalogType = CatalogTypes.BLOOD_TYPE,
                    Code = "AB-",
                    Name = "AB Negativo",
                    DisplayOrder = 8,
                    CreatedBy = "System"
                }
            ];
    }

    private static List<MasterCatalog> GetFlightPositions()
    {
        return
            [
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "PA",
                    Name = "Piloto Alumno",
                    DisplayOrder = 1,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "CP",
                    Name = "Copiloto",
                    DisplayOrder = 2,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "PI",
                    Name = "Piloto",
                    DisplayOrder = 3,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "PC",
                    Name = "Piloto en Comando",
                    DisplayOrder = 4,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "PS",
                    Name = "Piloto de Seguridad",
                    DisplayOrder = 5,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "MP",
                    Name = "Piloto de Mantenimiento",
                    DisplayOrder = 6,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "IP",
                    Name = "Piloto Instructor",
                    DisplayOrder = 7,
                    CreatedBy = "System"
                },
                new MasterCatalog
                {
                    CatalogType = CatalogTypes.FLIGHT_POSITION,
                    Code = "SP",
                    Name = "Piloto Estandarizador",
                    DisplayOrder = 8,
                    CreatedBy = "System"
                }
            ];
    }

}