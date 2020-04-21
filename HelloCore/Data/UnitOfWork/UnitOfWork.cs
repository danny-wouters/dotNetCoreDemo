using HelloCore.Data.Repository;
using HelloCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloCore.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HelloCoreContext _context;
        private IGenericRepository<Bestelling> _bestellingRepository;
        private IGenericRepository<Klant> _klantRepository;

        public UnitOfWork(HelloCoreContext context)
        {
            _context = context;
        }

        public IGenericRepository<Bestelling> BestellingRepository
        {
            get
            {
                if (this._bestellingRepository == null)
                {
                    this._bestellingRepository = new GenericRepository<Bestelling>(_context);
                }
                return _bestellingRepository;
            }
        }

        public IGenericRepository<Klant> KlantRepository
        {
            get
            {
                if (this._klantRepository == null)
                {
                    this._klantRepository = new GenericRepository<Klant>(_context);
                }
                return _klantRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
