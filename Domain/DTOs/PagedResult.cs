namespace challenge.Domain.DTOs
{
    /// <summary>
    /// Representa um resultado paginado de uma consulta
    /// </summary>
    /// <typeparam name="T">Tipo dos itens retornados</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Lista de itens da página atual
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Página atual
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Tamanho da página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de itens
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        /// <summary>
        /// Indica se há página anterior
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Indica se há próxima página
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Links de navegação HATEOAS
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }

    /// <summary>
    /// Parâmetros para paginação
    /// </summary>
    public class PagingParameters
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        /// <summary>
        /// Número da página (começa em 1)
        /// </summary>
        public int PageNumber 
        { 
            get => _pageNumber; 
            set => _pageNumber = value > 0 ? value : 1; 
        }

        /// <summary>
        /// Tamanho da página (máximo 100)
        /// </summary>
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = value > 0 && value <= 100 ? value : 10; 
        }
    }
}

