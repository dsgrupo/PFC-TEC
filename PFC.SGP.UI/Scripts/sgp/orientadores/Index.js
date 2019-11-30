$('#modalInfoOrientador').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var codigo = button.data('codigo-orientador') // Extract info from data-* attributes
    var nome = button.data('nome-orientador')
    var sobrenome = button.data('sobrenome-orientador')
    var telefone = button.data('telefone-orientador')
    var email = button.data('email-orientador')
    // If necessary, we could initiate an AJAX request here (and then do the updating in a callback).
    var modal = $(this)
    modal.find('#modal-title-orientador').text('Detalhes do orientador "' + nome + '"')
    modal.find('#txtCodigo').val(codigo)
    modal.find('#txtNomeOrientador').val(nome)
    modal.find('#txtSobrenomeOrientador').val(sobrenome)
    modal.find('#txtTelefoneOrientador').val(telefone)
    modal.find('#txtEmailOrientador').val(email)
})

function ConfirmDelOrientador(id, nome, cod) {

    $('#nomeOrientadorModal').text(nome + " (Código: " + cod + ")")
    $('#delModal').data('id', id).modal('show')
}

function DelOrientador() {
    let id = $('#delModal').data('id');
    let xhr = new XMLHttpRequest(); // Api para requisição ajax
    const url = `/Orientadores/DelOrientador/${id}`

    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 404) {
            toastr.error('Orientador não encontrado na base de dados.', 'Operação mal-sucedida')
            $('#delModal').data('id', '0').modal('hide')
        }
        if (this.readyState === 4 && this.status === 400) {
            $('#delModal').data('id', '0').modal('hide')
            toastr.error('Não se pode deletar um orientador que orienta algum trabalho ativo.', 'Operação mal-sucedida')
        }
        if (this.readyState === 4 && this.status === 200) {
            let tr = document.querySelector(`#orientador-${id}`);
            if (tr != null) {
                let id = $('#delModal').data('id');
                var table = $('#table_id').DataTable();
                table.row(`#orientador-${id}`).remove().draw()
            }
            $('#delModal').data('id', '0').modal('hide')
            toastr.success('Orientador excluído da base de dados.', 'Operação bem sucedida')
        }
    }

    xhr.open('get', url) // Iniciar Solicitação | 'via url'
    xhr.send() // Envia para o servidor
}

$(document).ready(function () {
    myTable = $('#table_id').DataTable({
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