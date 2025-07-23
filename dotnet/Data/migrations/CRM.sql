CREATE TABLE contactos_crm (
    co_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    co_nombre VARCHAR(255) NULL,
    co_mail VARCHAR(255) NULL,
    co_telefono VARCHAR(50) NULL,
    co_ruc VARCHAR(20) NULL,
    co_notas TEXT NULL,
    co_empresa VARCHAR(255) NULL,
    co_cargo VARCHAR(100) NULL,
    co_fecha_contacto DATETIME NOT NULL,
    co_es_cliente INT UNSIGNED NOT NULL DEFAULT 0,
    co_departamento INT UNSIGNED NOT NULL,
    co_ciudad INT UNSIGNED NOT NULL,
    co_zona INT UNSIGNED NOT NULL,
    co_direccion VARCHAR(500) NULL,
    co_estado INT NOT NULL DEFAULT 1,
    co_operador INT UNSIGNED NOT NULL,
    co_general INT NOT NULL DEFAULT 0,
    
    -- Claves foráneas
    FOREIGN KEY (co_departamento) REFERENCES departamentos(dep_codigo),
    FOREIGN KEY (co_ciudad) REFERENCES ciudades(ciu_codigo),
    FOREIGN KEY (co_zona) REFERENCES zonas(zo_codigo),
    FOREIGN KEY (co_operador) REFERENCES operadores(op_codigo)
);

-- Índices para mejorar el rendimiento
CREATE INDEX idx_contactos_crm_departamento ON contactos_crm(co_departamento);
CREATE INDEX idx_contactos_crm_ciudad ON contactos_crm(co_ciudad);
CREATE INDEX idx_contactos_crm_zona ON contactos_crm(co_zona);
CREATE INDEX idx_contactos_crm_estado ON contactos_crm(co_estado);
CREATE INDEX idx_contactos_crm_fecha ON contactos_crm(co_fecha_contacto);

CREATE TABLE estados_crm (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(255) NULL
);

-- Índice para mejorar el rendimiento en búsquedas por descripción
CREATE INDEX idx_estados_crm_descripcion ON estados_crm(descripcion);


CREATE TABLE oportunidades_crm (
    op_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    op_cliente INT UNSIGNED NOT NULL,
    op_titulo VARCHAR(255) NULL,
    op_descripcion TEXT NULL,
    op_valor_negociacion DECIMAL(15,2) NOT NULL DEFAULT 0.00,
    op_fecha_inicio DATETIME NOT NULL,
    op_fecha_fin DATETIME NULL,
    op_operador INT UNSIGNED NOT NULL,
    op_estado INT UNSIGNED NOT NULL,
    op_general INT NOT NULL DEFAULT 0,
    
    -- Claves foráneas
    FOREIGN KEY (op_cliente) REFERENCES contactos_crm(co_codigo),
    FOREIGN KEY (op_operador) REFERENCES operadores(op_codigo),
    FOREIGN KEY (op_estado) REFERENCES estados_crm(id)
);

-- Índices para mejorar el rendimiento
CREATE INDEX idx_oportunidades_crm_cliente ON oportunidades_crm(op_cliente);
CREATE INDEX idx_oportunidades_crm_operador ON oportunidades_crm(op_operador);
CREATE INDEX idx_oportunidades_crm_estado ON oportunidades_crm(op_estado);
CREATE INDEX idx_oportunidades_crm_fecha_inicio ON oportunidades_crm(op_fecha_inicio);
CREATE INDEX idx_oportunidades_crm_valor ON oportunidades_crm(op_valor_negociacion);

CREATE TABLE tareas_crm (
    ta_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    ta_titulo VARCHAR(255) NULL,
    ta_descripcion TEXT NULL,
    ta_resultado TEXT NULL,
    ta_fecha DATETIME NOT NULL,
    ta_oportunidad INT UNSIGNED NOT NULL,
    ta_tipo_tarea INT UNSIGNED NOT NULL,
    ta_fecha_limite DATETIME NULL,
    ta_estado INT NOT NULL DEFAULT 1,
    
    -- Clave foránea
    FOREIGN KEY (ta_oportunidad) REFERENCES oportunidades_crm(op_codigo)
);

-- Índices para mejorar el rendimiento
CREATE INDEX idx_tareas_crm_oportunidad ON tareas_crm(ta_oportunidad);
CREATE INDEX idx_tareas_crm_fecha ON tareas_crm(ta_fecha);
CREATE INDEX idx_tareas_crm_estado ON tareas_crm(ta_estado);
CREATE INDEX idx_tareas_crm_tipo_tarea ON tareas_crm(ta_tipo_tarea);

CREATE TABLE agendamientos_crm (
    ag_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    ag_fecha_inicio DATETIME NOT NULL,
    ag_fecha_agendamiento DATETIME NOT NULL,
    ag_hora_agendamiento TIME NOT NULL,
    ag_titulo VARCHAR(255) NULL,
    ag_descripcion TEXT NULL,
    ag_doctor INT UNSIGNED NOT NULL,
    ag_paciente INT UNSIGNED NOT NULL,
    ag_cliente INT UNSIGNED NOT NULL,
    ag_operador INT UNSIGNED NOT NULL,
    ag_estado INT NOT NULL DEFAULT 1,
    
    FOREIGN KEY (ag_operador) REFERENCES operadores(op_codigo),
    FOREIGN KEY (ag_doctor) REFERENCES doctores(doc_codigo),
    FOREIGN KEY (ag_paciente) REFERENCES pacientes(pac_codigo)
);

CREATE INDEX idx_agendamientos_crm_operador ON agendamientos_crm(ag_operador);
CREATE INDEX idx_agendamientos_crm_doctor ON agendamientos_crm(ag_doctor);
CREATE INDEX idx_agendamientos_crm_paciente ON agendamientos_crm(ag_paciente);


CREATE TABLE tipo_tarea_crm (
    tipo_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    tipo_descripcion VARCHAR(255) NULL,
    tipo_estado INT UNSIGNED NOT NULL DEFAULT 1
);

-- Índice para mejorar el rendimiento en búsquedas por descripción
CREATE INDEX idx_tipo_tarea_crm_descripcion ON tipo_tarea_crm(tipo_descripcion);
CREATE INDEX idx_tipo_tarea_crm_estado ON tipo_tarea_crm(tipo_estado);


-- Agregar la clave foránea faltante en tareas_crm
ALTER TABLE tareas_crm 
ADD CONSTRAINT fk_tareas_tipo_tarea 
FOREIGN KEY (ta_tipo_tarea) REFERENCES tipo_tarea_crm(tipo_codigo);


-- Agregar un índice adicional para optimizar las consultas por tipo de tarea
CREATE INDEX idx_tareas_crm_tipo_tarea_fk ON tareas_crm(ta_tipo_tarea);


CREATE TABLE recordatorios_crm (
    re_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    re_titulo VARCHAR(255) NULL,
    re_descripcion TEXT NULL,
    re_fecha DATETIME NOT NULL,
    re_fecha_limite DATETIME NULL,
    re_hora TIME NULL,
    re_operador INT UNSIGNED NOT NULL,
    re_cliente INT UNSIGNED DEFAULT NULL,
    re_estado INT NOT NULL DEFAULT 1,
    re_tipo_recordatorio INT UNSIGNED NOT NULL,
    FOREIGN KEY (re_operador) REFERENCES operadores(op_codigo),
    FOREIGN KEY (re_tipo_recordatorio) REFERENCES tipo_tarea_crm(tipo_codigo)
);

-- Seeder para estados del CRM
-- Ejecutar este script si la tabla estados_crm está vacía

INSERT IGNORE INTO estados_crm (id, descripcion) VALUES
(1, 'En planeación'),
(2, 'En Negociación'),
(3, 'Concluidos'),
(4, 'Rechazada');

INSERT INTO tipo_tarea_crm (tipo_codigo, tipo_descripcion, tipo_estado) VALUES
(1, 'Llamada', 1),
(2, 'Reunión', 1),
(3, 'Email', 1),
(4, 'Seguimiento', 1),
(5, 'Propuesta', 1),
(6, 'WhatsApp', 1),
(7, 'Visita técnica', 1),
(8, 'Presentación', 1),
(9, 'Negociación', 1),
(10, 'Cierre de venta', 1),
(11, 'Capacitación', 1),
(12, 'Soporte técnico', 1);

CREATE TABLE proyectos_colaboradores_crm (
    pc_codigo INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    pc_proyecto INT UNSIGNED NOT NULL,
    pc_colaborador INT UNSIGNED NOT NULL,
    pc_estado INT NOT NULL DEFAULT 1,
    FOREIGN KEY (pc_proyecto) REFERENCES oportunidades_crm (op_codigo),
    FOREIGN KEY (pc_colaborador) REFERENCES operadores (op_codigo)
);

-- Agregar la columna op_autorizado_por a la tabla oportunidades_crm para ver quien movio la oportunidad a estado concluido (o a cualquier otro estado)
alter table oportunidades_crm add column op_autorizado_por int unsigned DEFAULT NULL;

alter table oportunidades_crm add CONSTRAINT fk_oportunidades_crm_autorizado_por
FOREIGN KEY (op_autorizado_por) REFERENCES operadores (op_codigo);



