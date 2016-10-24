a. Considerações técnicas da solução:

1. Aplicação desenvolvida no visual studio 2015 utilizando-se framework 4.5.2
2. Base de dados Sql server (LocalDB) embarcada na aplicação
3. ORM Entity Framwork 6 com mapeamento via Fluent API e migrations habilitado
4. Camada de domínio seguindo algumas práticas de DDD descritas por Eric Evans em sua obra
5. CRUDS ferindo a arquitetura fazendo referência direta a camada de acesso a dados uma vez que são recursos "data-driven"
visando a simplicidade de implementação e maior produtividade
6. Foram utilizados diversos componentes javascript de terceiros, bem como uma camada de mapeamento Grid -> Actions customizada de 
maneira não-obstrutiva, deixando a implementação das ações de crud mais declarativa
7. Testes unitários executados contra a camada de domínio utilzando o próprio framework de testes do visual studio
8. Layout responsivo baseado em bootstrap testado com Chrome 

b. Informações para execução

1. Pode-se executar a partir do visual studio ou publicar a aplicação em um servidor com IIS e o banco de dados em um servidor 
com SQL Server caso escolha a segunda opção sugiro utilizar o recurso de web deploy para publicação.

