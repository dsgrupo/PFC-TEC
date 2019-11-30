$('#modalInfoAluno').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var matricula = button.data('matricula-aluno') // Extract info from data-* attributes
    var semestre = button.data('semestre-aluno')
    var nome = button.data('nome-aluno')
    var sobrenome = button.data('sobrenome-aluno')
    var telefone = button.data('telefone-aluno')
    var email = button.data('email-aluno')
    // If necessary, we could initiate an AJAX request here (and then do the updating in a callback).
    var modal = $(this)
    modal.find('#modal-title-aluno').text('Detalhes do aluno "' + nome + '"')
    modal.find('#txtMatricula').val(matricula)
    modal.find('#txtSemestre').val(semestre)
    modal.find('#txtNomeAluno').val(nome)
    modal.find('#txtSobrenomeAluno').val(sobrenome)
    modal.find('#txtTelefoneAluno').val(telefone)
    modal.find('#txtEmailAluno').val(email)
})

function ConfirmDelAluno(id, nome, mat) {

    $('#nomeAlunoModal').text(nome + " (Matrícula: " + mat + ")")
    $('#delModal').data('id', id).modal('show')
}

function DelAluno() {
    let id = $('#delModal').data('id');
    let xhr = new XMLHttpRequest(); // Api para requisição ajax
    const url = `/Alunos/DelAluno/${id}`

    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 404) {
            toastr.error('Aluno não encontrado na base de dados.', 'Operação mal-sucedida')
            $('#delModal').data('id', '0').modal('hide')
        }
        if (this.readyState === 4 && this.status === 400) {
            $('#delModal').data('id', '0').modal('hide')
            toastr.error('Não se pode deletar um aluno que possua algum trabalho ativo.', 'Operação mal-sucedida')
        }
        if (this.readyState === 4 && this.status === 200) {
            let tr = document.querySelector(`#aluno-${id}`);
            if (tr != null) {
                let id = $('#delModal').data('id');
                var table = $('#table_id').DataTable();
                table.row(`#aluno-${id}`).remove().draw()
            }
            $('#delModal').data('id', '0').modal('hide')
            toastr.success('Aluno excluído da base de dados.', 'Operação bem sucedida')
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