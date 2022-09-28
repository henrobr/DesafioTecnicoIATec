namespace Api.Classes
{
    public class ModelValidation
    {
        public List<ValidationResult> Result(object model)
        {
            var resultadoValidacao = new List<ValidationResult>();
            var contexto = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, contexto, resultadoValidacao, true);

            return resultadoValidacao;
        }
    }
}
