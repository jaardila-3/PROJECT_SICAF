namespace SICAF.Common.Models.Results;

/// <summary>
/// Representa el resultado de una operación que puede ser exitosa o fallar
/// </summary>
public class Result<T>
{
    private readonly T? _value;

    // Constructor privado para controlar la creación
    private Result(bool isSuccess, T? value, Error error)
    {
        IsSuccess = isSuccess;
        _value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    // Value lanza excepción si no es exitoso
    public T Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException($"No se puede acceder al valor de un resultado fallido. Error: {Error.Message}");

            return _value!;
        }
    }

    // Métodos para crear resultados
    public static Result<T> Success(T value) =>
        new(true, value, Error.None);

    public static Result<T> Failure(Error error) =>
        new(false, default, error);

    // Atajo para crear un error rápido
    public static Result<T> Failure(string code, string message) =>
        new(false, default, new Error(code, message));
}

/// <summary>
/// Result para operaciones que no retornan valor
/// </summary>
public class Result
{
    private Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(string code, string message) => new(false, new Error(code, message));
}

/***
📚 Resumen de Uso
1. Crear resultados exitosos
csharpreturn Result<UserDto>.Success(userDto);
return Result.Success(); // Sin valor

2. Crear resultados con error
csharp// Desde el catálogo
return Result<UserDto>.Failure(Errors.User.NotFound);

// Error personalizado
return Result<UserDto>.Failure("CUSTOM_ERROR", "Mensaje del error");

3. Verificar resultados
csharpif (result.IsSuccess)
{
    var value = result.Value;
    // Hacer algo con value
}
else
{
    var error = result.Error;
    _logger.LogError($"Error {error.Code}: {error.Message}");
}
*/