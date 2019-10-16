<p align="center">
  <img src="https://raw.githubusercontent.com/CorreiaEduardo/PFC-TEC/master/large-logo.png"/>
</p>
<hr/>
<blockquote>
O primeiro módulo do projeto final do curso técnico.
</blockquote>

### Conteúdos abordados
* [Sobre o PFC](#PFC)
* [Estrutura do Projeto](#Projeto)
* [Desenvolvedores](#Desenvolvedores)
* [Funcionalidades](#Funcionalidades)
* [Licença](#Licença)

### <a name="PFC"></a>Sobre o PFC
<p>Esse <strong>projeto final de curso</strong> em especifico se trata de uma ferramenta interna para o SENAI CIMATEC que visa controlar prazos dos trabalhos de conclusão de cursos. Também chamado de SGP (Sistema de gerenciamento de projetos), o sistema permite gerenciar trabalhos que devem ser apresentados, objetivando diminuir a ocorrência da perda de prazos e possibilitar um controle melhor por parte dos coordenadores, que podem visualizar dados dos orientadores, alunos e seus trabalhos, bem como das turmas.</p>

### <a name="Projeto"></a>Estrutura do Projeto
<p>A solução é dividida em alguns projetos, o projeto <strong>Data</strong> tem como dependência principal o Entity Framework, nesse projeto são realizadas as configurações das entidades a serem persistidas no banco, bem como a definição de algumas operações de persistência, merge, exclusão e busca. O projeto <strong>Domain</strong> comporta tudo - ou quase tudo - aquilo que não possui nenhuma dependência externa, e roda apenas com a referencia do assembly Microsoft.CSharp, nesse projeto estão interfaces que estabelecem contratos, entidades que são persistidas no banco, classes de segurança entre outros. O projeto <strong>UI</strong> é o projeto asp.net mvc, a camada visual da aplicação, que depende dos outros projetos, nesse projeto temos as views, controllers, viewmodels, arquivos css e js (customizados e de bibliotecas de terceiros, como o bootstrap) e arquivos de configuração.</p>

### <a name="Desenvolvedores"></a>Desenvolvedores
* "Eduardo Correia" <correiaeduardojr@gmail.com>
* "Felipe Bastos" <felipepradobastos@gmail.com>

### <a name="Funcionalidades"></a>Funcionalidades
- [x] Gerenciar trabalhos (Criar, atualizar, excluir, buscar com filtro);
- [x] Gerenciar alunos (Criar, atualizar, excluir, buscar com filtro);
- [x] Gerenciar orientadores (Criar, atualizar, excluir, buscar com filtro);
- [x] Gerenciar turmas (Criar, atualizar, excluir, buscar com filtro);
- [x] Gerenciar coordenadores (Criar, atualizar, excluir, buscar com filtro);
- [x] Gerenciar cursos (Criar, atualizar, excluir, buscar com filtro);
- [x] Obter resumo dos trabalhos a serem apresentados, como listas organizadas e categorizadas por proximidade da data final;
- [x] Obter resumo das movimentações de status dos trabalhos a serem apresentados, como listas organizadas e categorizadas por proximidade da data final.

### <a name="Licença"></a>Licença
<p><strong>MIT</strong> - "A short and simple permissive license with conditions only requiring preservation of copyright and license notices. Licensed works, modifications, and larger works may be distributed under different terms and without source code."</p>
