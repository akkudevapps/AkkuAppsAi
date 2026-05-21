(function() {
    'use strict';

    const AkkuUI = {
        init() {
            this.initSidebar();
            this.initRightSidebar();
            this.initTooltips();
            this.initStaggerAnimations();
            this.initMobileMenu();
            this.initSmoothScroll();
            this.initCardHoverEffects();
            this.initRippleEffect();
        },

        initSidebar() {
            const sidebar = document.getElementById('left-sidebar');
            const overlay = document.getElementById('sidebar-overlay');
            const toggles = document.querySelectorAll('[data-sidebar-toggle]');
            const closeBtn = document.getElementById('left-sidebar-close');

            const openSidebar = () => {
                sidebar.classList.add('open');
                overlay.classList.add('active');
                document.body.classList.add('sidebar-open');
            };

            const closeSidebar = () => {
                sidebar.classList.remove('open');
                overlay.classList.remove('active');
                document.body.classList.remove('sidebar-open');
            };

            toggles.forEach(btn => {
                btn.addEventListener('click', openSidebar);
            });

            if (closeBtn) {
                closeBtn.addEventListener('click', closeSidebar);
            }

            if (overlay) {
                overlay.addEventListener('click', closeSidebar);
            }

            document.addEventListener('keydown', (e) => {
                if (e.key === 'Escape' && sidebar.classList.contains('open')) {
                    closeSidebar();
                }
            });
        },

        initRightSidebar() {
            const sidebar = document.getElementById('right-sidebar');
            const toggles = document.querySelectorAll('[data-right-sidebar-toggle]');
            const closeBtn = document.getElementById('right-sidebar-close');
            const overlay = document.getElementById('sidebar-overlay');

            const openSidebar = () => {
                sidebar.classList.add('open');
                overlay.classList.add('active');
                document.body.classList.add('sidebar-open');
            };

            const closeSidebar = () => {
                sidebar.classList.remove('open');
                overlay.classList.remove('active');
                document.body.classList.remove('sidebar-open');
            };

            toggles.forEach(btn => {
                btn.addEventListener('click', openSidebar);
            });

            if (closeBtn) {
                closeBtn.addEventListener('click', closeSidebar);
            }

            if (overlay) {
                overlay.addEventListener('click', closeSidebar);
            }

            document.addEventListener('keydown', (e) => {
                if (e.key === 'Escape' && sidebar.classList.contains('open')) {
                    closeSidebar();
                }
            });
        },

        initMobileMenu() {
            const hamburger = document.querySelector('.hamburger');
            const navLinks = document.querySelector('.nav-links');
            const overlay = document.getElementById('mobile-overlay');

            if (hamburger && navLinks) {
                hamburger.addEventListener('click', () => {
                    navLinks.classList.toggle('open');
                    hamburger.classList.toggle('active');
                    if (overlay) overlay.classList.toggle('active');
                });

                if (overlay) {
                    overlay.addEventListener('click', () => {
                        navLinks.classList.remove('open');
                        hamburger.classList.remove('active');
                        overlay.classList.remove('active');
                    });
                }
            }
        },

        initTooltips() {
            const tooltips = document.querySelectorAll('[data-tooltip]');
            tooltips.forEach(el => {
                el.addEventListener('mouseenter', (e) => {
                    let tooltip = document.createElement('div');
                    tooltip.className = 'ui-tooltip';
                    tooltip.textContent = el.dataset.tooltip;
                    tooltip.style.cssText = `
                        position: absolute;
                        background: var(--elevated);
                        color: var(--txt);
                        padding: 6px 12px;
                        border-radius: 8px;
                        font-size: 12px;
                        white-space: nowrap;
                        z-index: 1000;
                        pointer-events: none;
                        box-shadow: 0 4px 12px rgba(0,0,0,0.3);
                        border: 1px solid var(--bdr);
                        animation: scaleIn 0.2s ease;
                    `;
                    document.body.appendChild(tooltip);
                    const rect = el.getBoundingClientRect();
                    tooltip.style.left = rect.left + (rect.width / 2) - (tooltip.offsetWidth / 2) + 'px';
                    tooltip.style.top = rect.top - tooltip.offsetHeight - 8 + 'px';
                    el._tooltip = tooltip;
                });
                el.addEventListener('mouseleave', () => {
                    if (el._tooltip) {
                        el._tooltip.remove();
                        el._tooltip = null;
                    }
                });
            });
        },

        initStaggerAnimations() {
            const staggerElements = document.querySelectorAll('.stagger-animate');
            staggerElements.forEach((el, i) => {
                el.style.animationDelay = `${i * 0.05}s`;
            });
        },

        initSmoothScroll() {
            document.querySelectorAll('a[href^="#"]').forEach(anchor => {
                anchor.addEventListener('click', function(e) {
                    const target = document.querySelector(this.getAttribute('href'));
                    if (target) {
                        e.preventDefault();
                        target.scrollIntoView({ behavior: 'smooth', block: 'start' });
                    }
                });
            });
        },

        initCardHoverEffects() {
            const cards = document.querySelectorAll('.card, .stat-card, .good-card, .tool-card');
            cards.forEach(card => {
                card.addEventListener('mousemove', (e) => {
                    const rect = card.getBoundingClientRect();
                    const x = e.clientX - rect.left;
                    const y = e.clientY - rect.top;
                    const centerX = rect.width / 2;
                    const centerY = rect.height / 2;
                    const rotateX = (y - centerY) / 20;
                    const rotateY = (centerX - x) / 20;
                    card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) translateY(-4px)`;
                });
                card.addEventListener('mouseleave', () => {
                    card.style.transform = '';
                });
            });
        },

        initRippleEffect() {
            document.querySelectorAll('.btn, .nav-btn, .admin-link').forEach(btn => {
                btn.addEventListener('click', function(e) {
                    const ripple = document.createElement('span');
                    ripple.className = 'ripple';
                    ripple.style.cssText = `
                        position: absolute;
                        border-radius: 50%;
                        background: rgba(255, 255, 255, 0.3);
                        width: 10px;
                        height: 10px;
                        pointer-events: none;
                        animation: rippleEffect 0.6s ease-out;
                    `;
                    this.style.position = this.style.position || 'relative';
                    this.style.overflow = 'hidden';
                    const rect = this.getBoundingClientRect();
                    ripple.style.left = e.clientX - rect.left + 'px';
                    ripple.style.top = e.clientY - rect.top + 'px';
                    this.appendChild(ripple);
                    setTimeout(() => ripple.remove(), 600);
                });
            });

            const style = document.createElement('style');
            style.textContent = `
                @keyframes rippleEffect {
                    to {
                        transform: scale(40);
                        opacity: 0;
                    }
                }
            `;
            document.head.appendChild(style);
        },

        showToast(message, type = 'info') {
            const toast = document.createElement('div');
            toast.className = `ui-toast ui-toast-${type}`;
            toast.innerHTML = `
                <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'times-circle' : 'info-circle'}"></i>
                <span>${message}</span>
            `;
            toast.style.cssText = `
                position: fixed;
                bottom: 20px;
                right: 20px;
                background: var(--elevated);
                color: var(--txt);
                padding: 12px 20px;
                border-radius: 12px;
                display: flex;
                align-items: center;
                gap: 10px;
                z-index: 9999;
                box-shadow: 0 8px 30px rgba(0,0,0,0.4);
                border: 1px solid var(--bdr);
                animation: slideRight 0.3s ease;
                font-size: 14px;
            `;
            if (type === 'success') toast.style.borderColor = 'var(--ok)';
            if (type === 'error') toast.style.borderColor = 'var(--err)';
            document.body.appendChild(toast);
            setTimeout(() => {
                toast.style.animation = 'slideRight 0.3s ease reverse';
                setTimeout(() => toast.remove(), 300);
            }, 3000);
        },

        confirmDialog(title, message) {
            return new Promise((resolve) => {
                const overlay = document.createElement('div');
                overlay.className = 'ui-dialog-overlay';
                overlay.style.cssText = `
                    position: fixed; inset: 0; background: rgba(0,0,0,0.7); z-index: 9998;
                    display: flex; align-items: center; justify-content: center;
                    animation: fadeSlideIn 0.2s ease;
                `;
                const dialog = document.createElement('div');
                dialog.className = 'ui-dialog';
                dialog.innerHTML = `
                    <div style="background:var(--panel);border:1px solid var(--bdr);border-radius:16px;padding:2rem;max-width:400px;width:90%;animation:scaleIn 0.3s ease;">
                        <h3 style="margin:0 0 0.5rem;">${title}</h3>
                        <p style="color:var(--txt2);margin-bottom:1.5rem;">${message}</p>
                        <div style="display:flex;gap:0.5rem;justify-content:flex-end;">
                            <button class="btn btn-ghost" id="dialog-cancel">Cancel</button>
                            <button class="btn btn-primary" id="dialog-confirm">Confirm</button>
                        </div>
                    </div>
                `;
                overlay.appendChild(dialog);
                document.body.appendChild(overlay);
                document.getElementById('dialog-cancel').addEventListener('click', () => { overlay.remove(); resolve(false); });
                document.getElementById('dialog-confirm').addEventListener('click', () => { overlay.remove(); resolve(true); });
                overlay.addEventListener('click', (e) => { if (e.target === overlay) { overlay.remove(); resolve(false); } });
            });
        }
    };

    document.addEventListener('DOMContentLoaded', () => AkkuUI.init());

    window.AkkuUI = AkkuUI;
})();
