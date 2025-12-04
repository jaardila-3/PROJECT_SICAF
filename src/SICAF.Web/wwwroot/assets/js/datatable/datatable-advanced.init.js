/**
 * Configuración estándar de DataTable para SICAF
 * @param {Object} options - Opciones adicionales para personalizar la configuración
 * @returns {Object} Configuración de DataTable
 */
window.getSICAFDataTableConfig = function (options = {}) {
  const defaultConfig = {
    dom: "Bfrtip",
    buttons: [
      {
        extend: 'copy',
        text: 'Copiar',
        exportOptions: {
          columns: ':not(.no-export)' // Excluye columnas con clase no-export
        }
      },
      {
        extend: 'csv',
        text: 'CSV',
        filename: options.filename || 'SICAF_' + new Date().toISOString().slice(0, 10),
        exportOptions: {
          columns: ':not(.no-export)'
        }
      },
      {
        extend: 'excel',
        text: 'Excel',
        filename: options.filename || 'SICAF_' + new Date().toISOString().slice(0, 10),
        title: options.title || 'Sistema SICAF',
        exportOptions: {
          columns: ':not(.no-export)'
        }
      },
      {
        extend: 'pdf',
        text: 'PDF',
        filename: options.filename || 'SICAF_' + new Date().toISOString().slice(0, 10),
        title: options.title || 'Sistema SICAF',
        orientation: 'landscape',
        pageSize: 'A4',
        exportOptions: {
          columns: ':not(.no-export)'
        }
      },
      {
        extend: 'print',
        text: 'Imprimir',
        title: options.title || 'Sistema SICAF',
        exportOptions: {
          columns: ':not(.no-export)'
        }
      }
    ],
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
  };

  // Combinar configuración por defecto con opciones personalizadas
  return { ...defaultConfig, ...options };
};

/**
 * Inicializa DataTable en un elemento con la configuración estándar de SICAF
 * @param {HTMLElement|string} tableElement - Elemento tabla o selector
 * @param {Object} options - Opciones adicionales
 * @returns {DataTable} Instancia de DataTable
 */
window.initSICAFDataTable = function (tableElement, options = {}) {
  const table = typeof tableElement === 'string'
    ? document.querySelector(tableElement)
    : tableElement;

  if (table && typeof DataTable !== 'undefined') {
    return new DataTable(table, window.getSICAFDataTableConfig(options));
  }
  return null;
};

// Inicializar automáticamente todas las tablas con la clase datatablesAdvancedJQuery
const tables = document.querySelectorAll(".datatablesAdvancedJQuery");
if (tables.length > 0) {
  tables.forEach(table => {
    window.initSICAFDataTable(table);
  });
}

// 
//    File export                              //
// 
$("#file_export").DataTable({
  dom: "Bfrtip",
  buttons: ["copy", "csv", "excel", "pdf", "print"],
});

// 
//    Buttons                                //
//
$(".buttons-copy, .buttons-csv, .buttons-print, .buttons-pdf, .buttons-excel").addClass("btn btn-primary");


// 
//  Show / hide columns dynamically                 //
// 

var table = $("#show_hide_col").DataTable({
  scrollY: "200px",
  paging: false,
});

$("a.toggle-vis").on("click", function (e) {
  e.preventDefault();

  // Get the column API object
  var column = $("#show_hide_col")
    .dataTable()
    .api()
    .column($(this).attr("data-column"));
  // Toggle the visibility
  column.visible(!column.visible());
});

// 
//    Column rendering                         //
// 
$("#col_render").DataTable({
  columnDefs: [
    {
      // The `data` parameter refers to the data for the cell (defined by the
      // `data` option, which defaults to the column being worked with, in
      // this case `data: 0`.
      render: function (data, type, row) {
        return data + " (" + row[3] + ")";
      },
      targets: 0,
    },
    { visible: false, targets: [3] },
  ],
});

// 
//     Row grouping                            //
// 
var table = $("#row_group").DataTable({
  pageLength: 10,
  columnDefs: [{ visible: false, targets: 2 }],
  order: [[2, "asc"]],
  displayLength: 25,
  drawCallback: function (settings) {
    var api = this.api();
    var rows = api.rows({ page: "current" }).nodes();
    var last = null;

    api
      .column(2, { page: "current" })
      .data()
      .each(function (group, i) {
        if (last !== group) {
          $(rows)
            .eq(i)
            .before(
              '<tr class="group"><td colspan="5">' + group + "</td></tr>"
            );

          last = group;
        }
      });
  },
});

// 
// Order by the grouping
// 
$("#row_group tbody").on("click", "tr.group", function () {
  var currentOrder = table.order()[0];
  if (currentOrder[0] === 2 && currentOrder[1] === "asc") {
    table.order([2, "desc"]).draw();
  } else {
    table.order([2, "asc"]).draw();
  }
});

// 
//    Multiple table control element           //
// 
$("#multi_control").DataTable({
  dom: '<"top"iflp<"clear">>rt<"bottom"iflp<"clear">>',
});

// 
//    DOM/jquery events                        //
// 
var table = $("#dom_jq_event").DataTable();

$("#dom_jq_event tbody").on("click", "tr", function () {
  var data = table.row(this).data();
  alert("You clicked on " + data[0] + "'s row");
});

// 
//    Language File                            //
// 
$("#lang_file").DataTable({
  language: {
    url: "../../assets/js/datatable/German.json",
  },
});

// 
//    Complex headers with column visibility   //
// 

$("#complex_head_col").DataTable({
  columnDefs: [
    {
      visible: false,
      targets: -1,
    },
  ],
});

// 
//    Setting defaults                         //
// 
var defaults = {
  searching: false,
  ordering: false,
};

$("#setting_defaults").dataTable($.extend(true, {}, defaults, {}));

// 
//    Footer callback                          //
// 
$("#footer_callback").DataTable({
  footerCallback: function (row, data, start, end, display) {
    var api = this.api(),
      data;

    // Remove the formatting to get integer data for summation
    var intVal = function (i) {
      return typeof i === "string"
        ? i.replace(/[\$,]/g, "") * 1
        : typeof i === "number"
          ? i
          : 0;
    };

    // Total over all pages
    total = api
      .column(4)
      .data()
      .reduce(function (a, b) {
        return intVal(a) + intVal(b);
      }, 0);

    // Total over this page
    pageTotal = api
      .column(4, { page: "current" })
      .data()
      .reduce(function (a, b) {
        return intVal(a) + intVal(b);
      }, 0);

    // Update footer
    $(api.column(4).footer()).html(
      "$" + pageTotal + " ( $" + total + " total)"
    );
  },
});

// 
//    Custom toolbar elements                  //
// 

$("#custom_tool_ele").DataTable({
  dom: '<"toolbar">frtip',
});

$("div.toolbar").html("<b>Custom tool bar! Text/images etc.</b>");

// 
//    Row created callback                     //
// 
$("#row_create_call").DataTable({
  createdRow: function (row, data, index) {
    if (data[5].replace(/[\$,]/g, "") * 1 > 150000) {
      $("td", row).eq(5).addClass("highlight");
    }
  },
});
