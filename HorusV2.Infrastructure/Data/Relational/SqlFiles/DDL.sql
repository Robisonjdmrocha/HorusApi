CREATE TABLE Historico_Solicitacoes_Transmissao
(
    id                     INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    identificador_unico    UNIQUEIDENTIFIER               NOT NULL,
    sigsm_id_usuario       INT                            NOT NULL,
    data_requisicao        DATETIME                       NOT NULL,
    data_atualizacao       DATETIME                       NULL,
    id_situacao_requisicao INT                            NOT NULL,
    situacao_requisicao    VARCHAR(30)                    NOT NULL,
    mensagem_auxiliar      VARCHAR(200)                   NULL,
    dia_solicitacao        INT                            NOT NULL,
    mes_solicitado         INT                            NOT NULL,
    ano_solicitado         INT                            NOT NULL
);

CREATE TABLE Tipo_Movimentacao
(
    id        INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    descricao VARCHAR(30)                    NOT NULL
);

INSERT INTO Tipo_Movimentacao
VALUES ('Entrada'),
       ('Saída'),
       ('Dispensação'),
       ('Posição de Estoque');

CREATE TABLE Movimentacoes_Por_Transmissao
(
    id                         INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    identificador_unico        UNIQUEIDENTIFIER               NOT NULL,
    id_solicitacao_transmissao INT REFERENCES Historico_Solicitacoes_Transmissao (id),
    id_tipo_movimentacao       INT REFERENCES Tipo_Movimentacao (id),
    data_transmissao           DATETIME,
    protocolo_horus            INT                            NOT NULL
);

-- Adicionar a regra de peso e altura para os medicamentos específicos

ALTER TABLE ALM_MovimentacaoDispensacao
    ADD Peso DECIMAL(3, 2), Altura INT;

CREATE TABLE HorusV2Catmat
(
    id INT PRIMARY KEY IDENTITY(1,1),
    descricao VARCHAR(250),
    codigo VARCHAR(15),
    id_tipo_medicamento INT
);