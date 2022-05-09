# Domain Driven Design

## Livros

Domain-Driven Design: Tackling Complexity in the Heart of Software - Eric Evans | Implementing Domain-Driven Design - Vaughn Vernon
:-------------------------:|:-------------------------:
![](https://dddcommunity.org/wp-content/uploads/files/images/cover_medium.jpg) | ![](https://dddcommunity.org/wp-content/uploads/2013/02/implementing-domain-driven-design-400x400-imae6dr5trk3uycd.jpeg)


O DDD é uma abordagem de modelagem de software com foco na complexidade da aplicação.
Através do conhecimento do domínio é possível facilitar a implementação de complexas regras / processos de negócio.

## **Complexidade de um Software**

- DDD é ou deve ser aplicado para casos de projetos de softwares complexos
- Grandes projetos possuem muitas áreas, muitas regras de negócio, muitas pessoas com diferentes visões em diferentes contextos
- Não há como não utilizar técnicas avançadas em projetos de alta complexidade
- Grande parte da complexidade desse tipo de software não vem da tecnologia, mas sim da comunicação, separação de contextos, entendimento do negócio por diversos ângulos
- Pessoas

## Pontos chave

- Ubiquitous Language
- Bounded Context
- Context Map

## Ubiquitous Language

- Vocabulário de todos os termos específicos do domínio
    - Nomes, verbos, adjetivos, jargões, apelidos, expressões idiomáticas e avérbios

## Compartilhado por todas as partes envolvidas no projeto
- Primeiro passo para evitar desentendimentos

## Dicas essenciais 

- Não precisamos definir (banco de dados, repositórios, security, cache...) são termos técnicos, essas parte podem ficar por fora e precisam ser mapeados na linguagem ubíqua
- Contratar/Escalar especialistas
- Lidar com siglas
    - Muito específico
    - Difíceis de lembrar
    - Evitar se possível
- O idioma dos termos
    - Apenas Português?
    - Idiomas estrangeiros?
- Sincronizar com o código fonte

## Bounded Context

- Contexto delimitado onde um elemento tem um significado bem defino
    - Todos os elementos da linguagem ubíqua
- Além dos limites do contexto a linguagem muda
    - Cada contexto delimitado tem sua própria linguagem ubíqua
- Domínio dividido em teia de contextos interconectados
    - Cada contexto delimitado tem sua arquitetura e implementação

## Context Map

- É modelo importante, onde definimos os nomes dos contextos, domínio Auxiliar, Principal, Genérico...
- Não existe um forma/padrão de fazer esse map

## Padrões de Context Mapping

- Partnership
- Shared Kernel
- Customer-Supplier Development
- Conformist
- Anticorruption-layer
- Open host service
- Published language
- Separate ways
- Big Ball of Mud

## Motivação

- Remover ambiguidade e duplicação
- Simplificar o design dos módulos
- Integração dos componentes externos

# Modelagem Tática e Patterns

## Elementos táticos
- Quando estamos falando de DDD e precisamos olhar mais a fundo um boded context. Precisamos ser capazes de modelarmos de forma assertiva os seus principais componentes, comportamentos e individualidades, bem como suas relações.

## Entidades
- É algo único que é capaz ser alterado de forma continua durante um longo período de tempo
- É algo que possui uma continuidade em seu ciclo de vida e pode ser distinguida independente dos atributos que são importantes para a aplicação do usuário. Pode ser uma pessoa, cidade, carro, um ticket de loteria ou uma transação bancária.
- Entidade = Identidade

## Value Objects
- Quando você se preocupa apenas com os atributos de um elemento de um model, classifique isso como um Value Object
- Trate o Value Object como imutável

## Aggregate
- Um agregado é um conjunto de objetos associados que tratamos como uma unidade para propósito de mudança de dados
- Se a entidade estiver em outro agregado a relação entre eles será por Id, caso seja do mesmo agregado, a relação será pela entidade

## Domain Services
- Um serviço de domínio é uma operação sem estado que cumpre uma tarefa especifica do domínio. Muitas vezes, a melhor indicação de que você deve criar um Serviço no modelo de domínio é quando a operação que você precisa executar parece não se encaixar como um método em um Agregado ou um Objeto de Valor
- Uma entidade pode realizar uma ação que vai afetar todas as entidades?
- Como realizar uma operação em lote?
- Como calcular algo cuja as informações constam em mais de uma entidade?

## Repositories
- Um repositório comumente se refere a um local de armazenamento, geralmente considerado um local de segurança ou preservação dos itens nele armazenados. Quando você armazena algo em um repositório e depois retorna para recuperá-lo, você espera que ele esteja no mesmo estado que estava quando você o colocou lá. Em algum momento, você pode optar por remover o item armazenado do repositório.
- Esses objetos semelhantes a coleções são sobre persistência. Todo tipo Agregado persistente terá um Repositório. De um modo geral, existe uma relação um-para-um entre um tipo Agregado em um Repositório.

## Domain Events
- Use um evento de domínio para capturar uma ocorrência de algo que aconteceu no domínio.
- A essência de um evento de domínio é que você usa para capturar coisas que podem desencadear uma mudança no estado do aplicativo que você está desenvolvendo. Esses objetos de evento são processados para causar alterações no sistema e armazenados para fornecer um AuditLog.
- Todo evento deve ser representado em uma ação realizada no passado:
    - UserCreated
    - OrderPlaced
    - EmailSent


## Módulos
- Em um contexto DDD, Módulos em seu modelo servem como contêineres nomeados para classes de objetos de domínio que são altamente coesas entre si. O objeto deve ser baixo acoplamento entre as classes que estão em módulos diferentes. Como os Módulos usados no DDD não são compartimentos de armazenamento anêmicos ou genéricos, também é importante nomear adequadamente os Módulos.
- Respeitar a linguagem Universal
- Baixo acoplamento
- Um ou mais agregados devem estar juntos somente se fazem sentido
- Organizado pelo domínio/subdomínio e não pelo tipo de objetos
- Devem respeitar a mesma divisão quando estão em camadas diferentes

## Factories
- Desloque a responsabilidade de criar instâncias do objetos complexos e AGRAGADOS para um objeto separado, que pode não ter responsabilidade no modelo de domínio, mas ainda faz parte do design do domínio. Forneça uma interface que encapsule toda a criação complexa e que não exija que o cliente faça referência às classes concretas dos objetos que estão sendo instanciados. Crie AGGREGATES inteiros de uma única vez, reforçando suas invariantes.

## Resumo

- DDD não é sair botando a mão no código, devemos:
    - Entender a complexidade do código
    - Mapear o domínio da aplicação
    - Criar uma linguagem universal
    - Criar o contexto (Bouded Context)
    - Conseguir separa o espaço do problema com espaço da solução
    - Depois de termos os elementos estratégicos definidos, podemos ir para parte tática