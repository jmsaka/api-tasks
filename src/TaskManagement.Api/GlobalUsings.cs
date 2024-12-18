﻿global using FluentValidation;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using NLog.Extensions.Logging;
global using System.Net;
global using TaskManagement.Api.Controllers.Base;
global using TaskManagement.Api.IoC;
global using TaskManagement.Application.Commands.Projetos;
global using TaskManagement.Application.Commands.Tarefas;
global using TaskManagement.Application.Profiles;
global using TaskManagement.Application.Queries.Projetos;
global using TaskManagement.Application.Queries.Tarefas;
global using TaskManagement.Application.Validators;
global using TaskManagement.Domain.Contracts;
global using TaskManagement.Domain.Dtos;
global using TaskManagement.Domain.Interfaces;
global using TaskManagement.Infrastructure.Data;
global using TaskManagement.Infrastructure.Repositories;
global using TaskManagement.Application.Commands.Comentarios;
global using TaskManagement.Application.Queries.Comentarios;
global using Microsoft.AspNetCore.Authorization;
global using TaskManagement.Application.Queries.Relatorios;
global using TaskManagement.Application.Queries.HistoricoAtualizacoes;