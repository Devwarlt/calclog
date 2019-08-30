using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calclog
{
    public class BooleanOperators
    {
        public bool negacao(bool p) => !p;
        public bool conjuncao(bool p, bool q) => p & q;
        public bool disjuncao(bool p, bool q) => p | q;
        public bool disjuncao_exclusiva(bool p, bool q) => p ^ q;
        public bool bicondicional(bool p, bool q) => negacao(disjuncao_exclusiva(p, q));
        public bool condicional(bool p, bool q) => negacao(conjuncao(p,negacao(q)));
    }
}
