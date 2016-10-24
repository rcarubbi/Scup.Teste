a. Considera��es t�cnicas da solu��o:

1. Aplica��o desenvolvida no visual studio 2015 utilizando-se framework 4.5.2
2. Base de dados Sql server (LocalDB) embarcada na aplica��o
3. ORM Entity Framwork 6 com mapeamento via Fluent API e migrations habilitado
4. Camada de dom�nio seguindo algumas pr�ticas de DDD descritas por Eric Evans em sua obra
5. CRUDS ferindo a arquitetura fazendo refer�ncia direta a camada de acesso a dados uma vez que s�o recursos "data-driven"
visando a simplicidade de implementa��o e maior produtividade
6. Foram utilizados diversos componentes javascript de terceiros, bem como uma camada de mapeamento Grid -> Actions customizada de 
maneira n�o-obstrutiva, deixando a implementa��o das a��es de crud mais declarativa
7. Testes unit�rios executados contra a camada de dom�nio utilzando o pr�prio framework de testes do visual studio
8. Layout responsivo baseado em bootstrap testado com Chrome 

b. Informa��es para execu��o

1. Pode-se executar a partir do visual studio ou publicar a aplica��o em um servidor com IIS e o banco de dados em um servidor 
com SQL Server caso escolha a segunda op��o sugiro utilizar o recurso de web deploy para publica��o.

