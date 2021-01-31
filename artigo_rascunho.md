# Maneiras de lidar com código legado
​
Esses dias eu estava lendo o livro "Working Effectively with Legacy Code" e o capítulo 6 (I don't have much time and a I have to change it) me chamou a atenção. Michael fala sobre um problema clássico que todos nós devs passamos diariamente, que é fazer uma alteração ou adicionar um novo comportamento em um sistema legado onde, muita das vezes, o local é repleto de dependências e muito difícil de testar. Pior: algumas vezes essas mudanças são importantes e de curto prazo, levando a nós a sacrificar o design e os testes unitários. Mas até que ponto esse sacrifício é válido?
​

Geralmente quebrar dependências e testar costuma ser algo demorado, leva-se um tempo. Ao olhar um código legado e com dependências, já nos questionamos do esforço a ser feito, principalmente porque há a possibilidade de nunca mais voltarmos naquele ponto. Só vamos saber que "valeu a pena" quando voltarmos lá e ver que aquele fluxo já foi refatorado e testado. Ou seja, enquanto sua primeira mudança durou 2 horas, a seguinte vai durar muito menos.
Você não sabe quanto tempo vai levar para refatorar e escrever os testes. Mas você também não sabe quanto tempo vai levar para debugar, se você cometeu um erro. Tempo que você poderia ter usado para fazer os testes.
​
​
* Não tenho muito tempo e preciso fazer alteração
* Mudanças ficam mais fáceis, não volta mais no mesmo lugar (perde-se mt tempo inicialmente, mas dps entende o benefício)
​
* Muito tempo em codigo legado e pouco em codigo novo faz você desistir de tentar melhorar e testar
​
---
- Exemplo ruim de refatoração automatica
- Problema resolvendo com os 4 conceitos abaixo:
Sprout method e class
Wrap method e class

**Exemplos**

O desenvolvedor recebeu a tarefa para incluir uma nova funcionalidade: um envio de e-mail para todo o usuário que for cadastrado. Quando ele olhou para o código, encontrou isto:

```csharp
    public class UserService
    {
        private readonly IConfiguration _configuration;
        public UserService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateUser(UserRequest request)
        {
            var user = new User();
            user.Name = request.Name;
            user.Email = request.Email;
            user.Password = request.Password;

            if(string.IsNullOrEmpty(user.Name) || user.Name.Length > 150)
                throw new Exception("Name invalid");

            if (string.IsNullOrEmpty(user.Email) || user.Email.Contains('@') == false || user.Email.Length > 100)
                throw new Exception("Email invalid");

            if (string.IsNullOrEmpty(user.Password) || user.Password.All(char.IsLetterOrDigit) == false || user.Email.Length > 40)
                throw new Exception("Password invalid");

            const string SQL = @"
            INSERT INTO User (Name, Email, Password)
            Values (@name, @email, @password)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServer")))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@name", user.Name, DbType.AnsiString);
                parameter.Add("@email", user.Email, DbType.AnsiString);
                parameter.Add("@password", user.Password, DbType.AnsiString);

                connection.Execute(SQL, parameter);
            }
        }
    }

```

Temos acima um fluxo de criação de usuário com um código mal organizado, com ações misturadas, grandes dependências e difícil de testar. Para adicionar a funcionalidade de envio de e-mail, o desenvolvedor poderia simplesmente escrever toda a lógica de envio após a inserção do banco. Certo? Mas dessa maneira, estaríamos aumentando ainda mais a complexidade do método, que já possui muitas responsabilidades e dependências.

Como resolver este problema com a técnica de Sprout?

## Sprout Method
------Uma nova alteração deve ser criada como um método novo. (novo comportamento)

Um novo comportamento deve ser criado em um novo método. Ou seja, ao invés de escrever todo o código no método de ```CreateUser```, o desenvolvedor adicionará essa nova lógica em um novo método ```SendNotificationUser()```, fazendo com que o ```CreateUser``` apenas chame-o e dando inicio a um código mais limpo.
 
### Solução

```csharp
public class UserService
{
    private readonly IConfiguration _configuration;
    public UserService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void CreateUser(UserRequest request)
    {
        var user = new User();
        user.Name = request.Name;
        user.Email = request.Email;
        user.Password = request.Password;

        if (string.IsNullOrEmpty(user.Name) || user.Name.Length > 150)
            throw new Exception("Name invalid");

        if (string.IsNullOrEmpty(user.Email) || user.Email.Contains('@') == false || user.Email.Length > 100)
            throw new Exception("Email invalid");

        if (string.IsNullOrEmpty(user.Password) || user.Password.All(char.IsLetterOrDigit) == false || user.Email.Length > 40)
            throw new Exception("Password invalid");

        const string SQL = @"
        INSERT INTO User (Name, Email, Password)
        Values (@name, @email, @password)";

        using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServer")))
        {
            var parameter = new DynamicParameters();
            parameter.Add("@name", user.Name, DbType.AnsiString);
            parameter.Add("@email", user.Email, DbType.AnsiString);
            parameter.Add("@password", user.Password, DbType.AnsiString);

            connection.Execute(SQL, parameter);
        }

        SendNotificationToUser(user);
    }

    private void SendNotificationToUser(User user)
    {
        //... code to send notification to User
    }
}
```

### Vantagens
- está claramente separando código antigo (legado) do novo;
- mesmo que você não o teste naquele momento, estará deixando a porta aberta para novas mudanças e testes futuros;

### Desvantagens
- quando você escolhe fazer um novo método, você está desistindo do método de origem naquele momento. Você não quer nem tentar testá-lo e tentar deixá-lo melhor;
- as vezes não fica claro o porquê toda a lógica está naquele método e apenas uma parte está em outra;

### Texto final

---
## Sprout Class
Uma nova alteração deve ser criada como um método novo. (novo comportamento)

### Solução
```csharp
public class UserService
{
    private readonly IConfiguration _configuration;
    private readonly INotification _notification;
    public UserService(
        IConfiguration configuration,
        INotification notification)
    {
        _configuration = configuration;
        _notification = notification;
    }

    public void CreateUser(UserRequest request)
    {
        var user = new User();
        user.Name = request.Name;
        user.Email = request.Email;
        user.Password = request.Password;

        if (string.IsNullOrEmpty(user.Name) || user.Name.Length > 150)
            throw new Exception("Name invalid");

        if (string.IsNullOrEmpty(user.Email) || user.Email.Contains('@') == false || user.Email.Length > 100)
            throw new Exception("Email invalid");

        if (string.IsNullOrEmpty(user.Password) || user.Password.All(char.IsLetterOrDigit) == false || user.Email.Length > 40)
            throw new Exception("Password invalid");

        const string SQL = @"
        INSERT INTO User (Name, Email, Password)
        Values (@name, @email, @password)";

        using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServer")))
        {
            var parameter = new DynamicParameters();
            parameter.Add("@name", user.Name, DbType.AnsiString);
            parameter.Add("@email", user.Email, DbType.AnsiString);
            parameter.Add("@password", user.Password, DbType.AnsiString);

            connection.Execute(SQL, parameter);
        }

        _notification.Send(user);
    }
}
```

### Vantagens
- está claramente separando código antigo (legado) do novo;
- mesmo que você não o teste naquele momento, você está deixando a porta aberta para novas mudanças e testes futuros;

### Desvantagens
- quando você escolhe fazer um novo método, você está desistindo do método de origem no momento. Você não quer nem tentar testá-lo e tentar deixá-lo melhor;
- as vezes não fica claro o porque toda a lógica está naquele método e apenas uma parte está em outra;
- 

### Exemplo em código

### Texto final

---
## Wrap Method 
Uma nova alteração deve ser criada como um método novo. (novo comportamento)

### Solução


### Vantagens
- está claramente separando código antigo (legado) do novo;
- mesmo que você não o teste naquele momento, você está deixando a porta aberta para novas mudanças e testes futuros;

### Desvantagens
- quando você escolhe fazer um novo método, você está desistindo do método de origem no momento. Você não quer nem tentar testá-lo e tentar deixá-lo melhor;
- as vezes não fica claro o porque toda a lógica está naquele método e apenas uma parte está em outra;
- 

### Exemplo em código

### Texto final

---
## Wrap Class 
Uma nova alteração deve ser criada como um método novo. (novo comportamento)

### Vantagens
- está claramente separando código antigo (legado) do novo;
- mesmo que você não o teste naquele momento, você está deixando a porta aberta para novas mudanças e testes futuros;

### Desvantagens
- quando você escolhe fazer um novo método, você está desistindo do método de origem no momento. Você não quer nem tentar testá-lo e tentar deixá-lo melhor;
- as vezes não fica claro o porque toda a lógica está naquele método e apenas uma parte está em outra;
- 

### Exemplo em código

### Texto final