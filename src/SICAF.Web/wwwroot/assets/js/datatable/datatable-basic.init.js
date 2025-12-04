const tables = document.querySelectorAll(".datatablesJQuery");
if (tables.length > 0) {
  tables.forEach(table => {
    new DataTable(table, {
      order: [], // Esto mantiene el orden que viene del servidor
      responsive: true,
      language: {
        decimal: ",",
        thousands: ".",
        emptyTable: "No hay datos disponibles",
        info: "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        infoEmpty: "Mostrando registros del 0 al 0 de un total de 0 registros",
        infoFiltered: "(filtrado de un total de _MAX_ registros)",
        zeroRecords: "No se encontraron resultados",
        infoPostFix: "",
        lengthMenu: "Mostrar _MENU_ registros",
        loadingRecords: "Cargando...",
        search: "Buscar:",
        searchPlaceholder: "Buscar...",
        paginate: {
          first: "Primero",
          last: "Último",
          next: "Siguiente",
          previous: "Anterior"
        },
        processing: "Procesando...",
        aria: {
          sortAscending: ": Activar para ordenar la columna de manera ascendente",
          sortDescending: ": Activar para ordenar la columna de manera descendente"
        }
      }
    });
  });
}


/****************************************
 *         Table Responsive             *
 ****************************************/
$(function () {
  $("#config-table").DataTable({
    responsive: true,
  });
});

/****************************************
 *       Basic Table                   *
 ****************************************/
$("#zero_config").DataTable();

/****************************************
 *       Default Order Table           *
 ****************************************/
$("#default_order").DataTable({
  order: [[3, "desc"]],
});

/****************************************
 *       Multi-column Order Table      *
 ****************************************/
$("#multi_col_order").DataTable({
  columnDefs: [
    {
      targets: [0],
      orderData: [0, 1],
    },
    {
      targets: [1],
      orderData: [1, 0],
    },
    {
      targets: [4],
      orderData: [4, 0],
    },
  ],
});

/****************************************
 *       Complex header Table          *
 ****************************************/
$("#complex_header").DataTable();

/****************************************
 *       DOM positioning Table         *
 ****************************************/
$("#DOM_pos").DataTable({
  dom: '<"top"i>rt<"bottom"flp><"clear">',
});

/****************************************
 *     alternative pagination Table    *
 ****************************************/
$("#alt_pagination").DataTable({
  pagingType: "full_numbers",
});

/****************************************
 *     vertical scroll Table    *
 ****************************************/
$("#scroll_ver").DataTable({
  scrollY: "300px",
  scrollCollapse: true,
  paging: false,
});

/****************************************
 * vertical scroll,dynamic height Table *
 ****************************************/
$("#scroll_ver_dynamic_hei").DataTable({
  scrollY: "50vh",
  scrollCollapse: true,
  paging: false,
});

/****************************************
 *     horizontal scroll Table    *
 ****************************************/
$("#scroll_hor").DataTable({
  scrollX: true,
});

/****************************************
 * vertical & horizontal scroll Table  *
 ****************************************/
$("#scroll_ver_hor").DataTable({
  scrollY: 300,
  scrollX: true,
});

/****************************************
 * Language - Comma decimal place Table  *
 ****************************************/
$("#lang_comma_deci").DataTable({
  language: {
    decimal: ",",
    thousands: ".",
  },
});

/****************************************
 *         Language options Table      *
 ****************************************/
$("#lang_opt").DataTable({
  language: {
    lengthMenu: "Display _MENU_ records per page",
    zeroRecords: "Nothing found - sorry",
    info: "Showing page _PAGE_ of _PAGES_",
    infoEmpty: "No records available",
    infoFiltered: "(filtered from _MAX_ total records)",
  },
});
