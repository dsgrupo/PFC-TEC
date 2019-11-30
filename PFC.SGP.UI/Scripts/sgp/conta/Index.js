$('#modalDetalhesCoordenador').on('show.bs.modal', function (event) {
    var rowClicked = $(event.relatedTarget) // Row that triggered the modal
    var login = rowClicked.data('login') // Extract info from data-* attributes
    var email = rowClicked.data('email')
    var nome = rowClicked.data('nome')
    var telefone = rowClicked.data('telefone')
    // If necessary, we could initiate an AJAX request here (and then do the updating in a callback).
    var modal = $(this)
    modal.find('#txtDetModalTitle').text('Detalhes do coordenador "' + nome + '"')
    modal.find('#txtDetLogin').val(login)
    modal.find('#txtDetEmail').val(email)
    modal.find('#txtDetTelefone').val(telefone)
})

function modalCurso(nome) {
    document.getElementById("txtCurso").value = nome;
    $('#modalCursos').modal('hide')
}

function ConfirmDelCoord(id, nome, cod) {

    $('#nomeCoordModal').text(nome + " (Código: " + cod + ")")
    $('#delModal').data('id', id).modal('show')
}

function DelCoord() {
    let id = $('#delModal').data('id');
    let xhr = new XMLHttpRequest(); // Api para requisição ajax
    const url = `/Conta/DelCoord/${id}`

    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 404) {
            toastr.error('Coordenador não encontrado na base de dados.', 'Operação mal-sucedida')
            $('#delModal').data('id', '0').modal('hide')
        }
        if (this.readyState === 4 && this.status === 400) {
            $('#delModal').data('id', '0').modal('hide')
            toastr.error('Não se pode deletar um coordenador associado a um curso ativo.', 'Operação mal-sucedida')
        }
        if (this.readyState === 4 && this.status === 200) {
            let tr = document.querySelector(`#coordenador-${id}`);
            if (tr != null) {
                let id = $('#delModal').data('id');
                var table = $('#table_id').DataTable();
                table.row(`#coordenador-${id}`).remove().draw()
            }
            $('#delModal').data('id', '0').modal('hide')
            toastr.success('Coordenador excluído da base de dados.', 'Operação bem sucedida')
        }
    }

    xhr.open('get', url) // Iniciar Solicitação | 'via url'
    xhr.send() // Envia para o servidor
}

$(document).ready(function () {
    $('#table_id').dataTable({
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });
});