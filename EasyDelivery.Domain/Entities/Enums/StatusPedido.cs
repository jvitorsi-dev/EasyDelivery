using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities.Enums
{
    public enum StatusPedido
    {
        Criado,
        PagamentoPendente,
        Pago,
        EmPreparacao,
        AguardandoEntregador,
        EmEntrega,
        Entregue,
        Cancelado
    }
}
