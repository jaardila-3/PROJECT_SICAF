using System.Reflection;

using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Common;
using SICAF.Common.DTOs.Identity;
using SICAF.Data.Entities.Identity;

namespace SICAF.Business.Mappers.Identity;

public static class UserMapperExtensions
{
    /// <summary>
    /// Mapea las propiedades de UserBase entre DTOs usando reflexión
    /// </summary>
    /// <typeparam name="TSource">Tipo de origen que hereda de UserBase</typeparam>
    /// <typeparam name="TDestination">Tipo de destino que hereda de UserBase</typeparam>
    /// <param name="source">Objeto origen</param>
    /// <param name="destination">Objeto destino (opcional, se crea uno nuevo si es null)</param>
    /// <returns>Objeto destino con las propiedades mapeadas</returns>
    public static TDestination MapUserBaseProperties<TSource, TDestination>(
        this TSource source,
        TDestination? destination = null)
        where TSource : UserBase
        where TDestination : UserBase, new()
    {
        ArgumentNullException.ThrowIfNull(source);

        destination ??= new TDestination();

        var baseType = typeof(UserBase);
        var properties = baseType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => p.CanRead && p.CanWrite);

        foreach (var property in properties)
        {
            var value = property.GetValue(source);
            property.SetValue(destination, value);
        }

        return destination;
    }

    /// <summary>
    /// Mapea la entidad User a UserDto
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static UserDto MapToDto(this User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            DocumentType = user.DocumentType,
            IdentificationNumber = user.IdentificationNumber,
            Grade = user.Grade,
            GradeOrder = GetGradeOrder(user.Grade),
            SeniorityOrder = user.SeniorityOrder,
            Email = user.Email,
            Nationality = user.Nationality,
            BirthDate = user.BirthDate,
            BirthDateString = user.BirthDate.ToString("yyyy-MM-dd"),
            Force = user.Force,
            PhotoData = user.PhotoData,
            PhotoContentType = user.PhotoContentType,
            PhotoFileName = user.PhotoFileName,
            BloodType = user.BloodType,
            LockoutEnd = user.LockoutEnd,
            LockoutReason = user.LockoutReason,
            IsPasswordSetByAdmin = user.IsPasswordSetByAdmin,
            Roles = user.UserRoles?.Select(ur => ur.Role!.MapToDto()).ToList() ?? []
        };

        // Mapear perfil de aviación si existe
        if (user.AviationProfile != null)
        {
            userDto.AviationProfile = new AviationProfileDto
            {
                Id = user.AviationProfile.Id,
                UserId = user.AviationProfile.UserId,
                PID = user.AviationProfile.PID,
                FlightPosition = user.AviationProfile.FlightPosition,
                WingType = user.AviationProfile.WingType
            };
        }

        return userDto;
    }

    /// <summary>
    /// Actualiza las propiedades de User con las propiedades de UserBase o la clase derivada
    /// </summary>
    /// <param name="user"></param>
    /// <param name="dto"></param>
    public static void UpdateFromDto(this User user, UserBase dto)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(dto);

        // Solo actualizar propiedades que deben cambiar
        user.Username = dto.Username;
        user.PhoneNumber = dto.PhoneNumber;
        user.DocumentType = dto.DocumentType.ToUpper();
        user.Grade = dto.Grade;
        user.SeniorityOrder = dto.SeniorityOrder;
        user.Email = dto.Email;
        user.Nationality = dto.Nationality;
        user.BloodType = dto.BloodType;
        user.BirthDate = dto.BirthDate;
        user.Force = dto.Force;
        user.LockoutEnd = dto.LockoutEnd;
        user.LockoutReason = dto.LockoutReason;

        // Actualizar foto
        if (dto.PhotoData != null)
        {
            user.PhotoData = dto.PhotoData;
            user.PhotoContentType = dto.PhotoContentType;
            user.PhotoFileName = dto.PhotoFileName;
        }
    }

    /// <summary>
    /// Mapea UserBase o clase derivada a User
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static User MapToEntity(this UserBase dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new User
        {
            Name = dto.Name.ToUpper(),
            LastName = dto.LastName.ToUpper(),
            Username = dto.Username,
            PhoneNumber = dto.PhoneNumber,
            DocumentType = dto.DocumentType.ToUpper(),
            IdentificationNumber = dto.IdentificationNumber,
            Grade = dto.Grade,
            SeniorityOrder = dto.SeniorityOrder,
            Email = dto.Email,
            Nationality = dto.Nationality,
            BloodType = dto.BloodType,
            BirthDate = dto.BirthDate,
            Force = dto.Force,
            PhotoData = dto.PhotoData,
            PhotoContentType = dto.PhotoContentType,
            PhotoFileName = dto.PhotoFileName,
            LockoutEnd = dto.LockoutEnd,
            LockoutReason = dto.LockoutReason
        };
    }

    public static int GetGradeOrder(string? gradeCode)
    {
        if (string.IsNullOrEmpty(gradeCode))
            return 999;

        var gradeOrders = new Dictionary<string, int>
        {
            { "CR", 1 },  // Coronel
            { "TC", 2 },  // Teniente Coronel
            { "MY", 3 },  // Mayor
            { "CT", 4 },  // Capitán
            { "TE", 5 },  // Teniente
            { "ST", 6 },  // Subteniente
            { "CM", 7 },  // Comisario
            { "SC", 8 },  // Subcomisario
            { "IJ", 9 },  // Intendente Jefe
            { "IT", 10 }, // Intendente
            { "SI", 11 }, // Subintendente
            { "PT", 12 }, // Patrullero
            { "NU", 13 }  // No Uniformado
        };

        return gradeOrders.TryGetValue(gradeCode, out var order) ? order : 999;
    }
}