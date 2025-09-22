using challenge.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace challenge.Infrastructure.Services
{
    /// <summary>
    /// Serviço para geração de links HATEOAS
    /// </summary>
    public interface IHateoasService
    {
        /// <summary>
        /// Gera links HATEOAS para um recurso específico
        /// </summary>
        List<LinkDto> GenerateResourceLinks(string resourceName, Guid resourceId, IUrlHelper urlHelper);

        /// <summary>
        /// Gera links HATEOAS para listas paginadas
        /// </summary>
        List<LinkDto> GeneratePaginationLinks<T>(PagedResult<T> pagedResult, string resourceName, IUrlHelper urlHelper);
    }

    public class HateoasService : IHateoasService
    {
        public List<LinkDto> GenerateResourceLinks(string resourceName, Guid resourceId, IUrlHelper urlHelper)
        {
            var links = new List<LinkDto>();

            // Link para o próprio recurso
            links.Add(new LinkDto(
                urlHelper.Link("Get" + resourceName, new { id = resourceId }) ?? "",
                "self",
                "GET"
            ));

            // Link para atualização
            links.Add(new LinkDto(
                urlHelper.Link("Update" + resourceName, new { id = resourceId }) ?? "",
                "update",
                "PUT"
            ));

            // Link para exclusão
            links.Add(new LinkDto(
                urlHelper.Link("Delete" + resourceName, new { id = resourceId }) ?? "",
                "delete",
                "DELETE"
            ));

            return links;
        }

        public List<LinkDto> GeneratePaginationLinks<T>(PagedResult<T> pagedResult, string resourceName, IUrlHelper urlHelper)
        {
            var links = new List<LinkDto>();

            // Link para a primeira página
            if (pagedResult.HasPrevious)
            {
                links.Add(new LinkDto(
                    urlHelper.Link("Get" + resourceName + "s", new { pageNumber = 1, pageSize = pagedResult.PageSize }) ?? "",
                    "first",
                    "GET"
                ));
            }

            // Link para página anterior
            if (pagedResult.HasPrevious)
            {
                links.Add(new LinkDto(
                    urlHelper.Link("Get" + resourceName + "s", new { pageNumber = pagedResult.CurrentPage - 1, pageSize = pagedResult.PageSize }) ?? "",
                    "prev",
                    "GET"
                ));
            }

            // Link para próxima página
            if (pagedResult.HasNext)
            {
                links.Add(new LinkDto(
                    urlHelper.Link("Get" + resourceName + "s", new { pageNumber = pagedResult.CurrentPage + 1, pageSize = pagedResult.PageSize }) ?? "",
                    "next",
                    "GET"
                ));
            }

            // Link para última página
            if (pagedResult.HasNext)
            {
                links.Add(new LinkDto(
                    urlHelper.Link("Get" + resourceName + "s", new { pageNumber = pagedResult.TotalPages, pageSize = pagedResult.PageSize }) ?? "",
                    "last",
                    "GET"
                ));
            }

            return links;
        }
    }
}

